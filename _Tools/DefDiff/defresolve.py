"""Resolve FI defs the way RimWorld does (load folders -> XML inheritance) and dump a
canonical form per def, so a refactor can be checked for zero behavioural diff.

Usage: defresolve.py <repo> <mode: vanilla|ce> <out.json>
Only FI's own abstract parents are resolved; external parents (BuildingBase...) are kept
as the 'external_parent' marker. Patches are not applied (they are unchanged by refactor
and are checked separately)."""
import sys, glob, os, json, copy
import xml.etree.ElementTree as ET

def folders(mode):
    f = ['1.6/Defs']
    f.append('1.6/CombatExtended/Defs' if mode == 'ce' else '1.6/Vanilla/Defs')
    return f

def load(repo, mode):
    defs = []
    for fo in folders(mode):
        for p in sorted(glob.glob(os.path.join(repo, fo, '*.xml'))):
            root = ET.parse(p).getroot()
            for el in root:
                if isinstance(el.tag, str):
                    defs.append(el)
    return defs

def is_list(node):
    kids = [c for c in node if isinstance(c.tag, str)]
    return len(kids) > 0 and all(c.tag == 'li' for c in kids)

def merge(cur, child):
    """Port of Verse.XmlInheritance.RecursiveNodeCopyOverwriteElements."""
    if child.get('Inherit', '').lower() == 'false':
        for c in list(cur): cur.remove(c)
        cur.text = child.text
        for c in child: cur.append(copy.deepcopy(c))
        for k, v in child.attrib.items():
            if k != 'Inherit': cur.set(k, v)
        return cur
    cur.attrib.clear()                      # child attributes replace the parent's
    for k, v in child.attrib.items(): cur.set(k, v)
    ckids = [c for c in child if isinstance(c.tag, str)]
    if (child.text or '').strip() and not ckids:   # text value overrides everything
        for c in list(cur): cur.remove(c)
        cur.text = child.text
        return cur
    if not ckids:                           # empty node: keep parent's elements, drop its text
        if not [c for c in cur if isinstance(c.tag, str)]:
            cur.text = None
        return cur
    for c in ckids:
        if c.tag == 'li':                   # list items are always appended
            cur.append(copy.deepcopy(c)); continue
        existing = cur.find(c.tag)
        if existing is not None:
            merge(existing, c)
        else:
            cur.append(copy.deepcopy(c))
    return cur

def resolve_all(defs):
    named = {}
    for el in defs:
        if el.get('Name'):
            named[(el.tag, el.get('Name'))] = el
    cache = {}
    def res(el):
        key = id(el)
        if key in cache: return cache[key]
        p = el.get('ParentName')
        if p and (el.tag, p) in named:
            base = copy.deepcopy(res(named[(el.tag, p)])[0])
            ext = res(named[(el.tag, p)])[1]
            out = merge(base, el)
        else:
            out = copy.deepcopy(el); ext = p
        cache[key] = (out, ext)
        return cache[key]
    result = {}
    for el in defs:
        if (el.get('Abstract') or '').lower() == 'true':
            continue
        dn = el.findtext('defName')
        if not dn: continue
        out, ext = res(el)
        for a in ('Name', 'ParentName', 'Abstract'):
            out.attrib.pop(a, None)
        result['%s/%s' % (el.tag, dn)] = {'external_parent': ext, 'xml': canon(out)}
    return result

def canon(el):
    def walk(e, ind=0):
        attrs = ''.join(' %s="%s"' % kv for kv in sorted(e.attrib.items()))
        kids = [c for c in e if isinstance(c.tag, str)]
        # field containers are order-independent in RimWorld; li lists keep their order
        if kids and not all(c.tag == 'li' for c in kids):
            kids = sorted(kids, key=lambda c: c.tag)
        elif kids and e.tag == 'damageMultipliers':
            kids = sorted(kids, key=lambda c: '\n'.join(walk(c)))
        elif kids and e.tag == 'comps':
            vis = {'Fortified.CompProperties_CastFlecker', 'Fortified.CompProperties_Flecker', 'MuzzleFlash.MuzzleFlashProps'}
            kids = [c for c in kids if c.get('Class') not in vis] + \
                   sorted([c for c in kids if c.get('Class') in vis], key=lambda c: '\n'.join(walk(c)))
        if not kids:
            t = (e.text or '').strip()
            return ['  ' * ind + '<%s%s>%s</%s>' % (e.tag, attrs, t, e.tag)]
        lines = ['  ' * ind + '<%s%s>' % (e.tag, attrs)]
        for c in kids: lines += walk(c, ind + 1)
        lines.append('  ' * ind + '</%s>' % e.tag)
        return lines
    return '\n'.join(walk(el))

if __name__ == '__main__':
    repo, mode, out = sys.argv[1:4]
    r = resolve_all(load(repo, mode))
    json.dump(r, open(out, 'w'), indent=0, sort_keys=True, ensure_ascii=False)
    print(mode, len(r), 'defs')
