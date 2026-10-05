import sys, json, difflib
a = json.load(open(sys.argv[1])); b = json.load(open(sys.argv[2]))
n = 0
for k in sorted(set(a) | set(b)):
    if k not in a: print('ADDED', k); n += 1; continue
    if k not in b: print('REMOVED', k); n += 1; continue
    if a[k] != b[k]:
        n += 1
        print('CHANGED', k)
        if a[k]['external_parent'] != b[k]['external_parent']:
            print('  parent', a[k]['external_parent'], '->', b[k]['external_parent'])
        for l in difflib.unified_diff(a[k]['xml'].splitlines(), b[k]['xml'].splitlines(), lineterm='', n=1):
            if l.startswith(('---', '+++')): continue
            print('  ' + l)
print('DIFF COUNT', n)
