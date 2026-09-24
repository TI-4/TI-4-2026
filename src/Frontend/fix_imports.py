import os
import re
from pathlib import Path

src_dir = Path('/home/nacho/Documents/university/uct-map/TI-4-2026/src/Frontend/src').resolve()

# Build map of filename -> absolute path
file_map = {}
for p in src_dir.rglob('*.*'):
    if p.suffix in ['.ts', '.tsx'] and 'assets' not in p.parts:
        # p.stem is the filename without extension
        file_map[p.stem] = p

import_regex = re.compile(r"(from\s+['\"]|import\s+['\"])(.*?)(['\"])")

for file_path in src_dir.rglob('*.*'):
    if file_path.suffix not in ['.ts', '.tsx'] or 'assets' in file_path.parts:
        continue

    content = file_path.read_text(encoding='utf-8')
    new_content = content
    changed = False

    for match in import_regex.finditer(content):
        prefix, import_path, suffix = match.groups()
        if not import_path.startswith('.'):
            continue
            
        # Extract the filename from the import path
        # e.g. '../../components/Button' -> 'Button'
        import_name = import_path.split('/')[-1]
        
        # If the filename exists in our map, calculate the correct relative path
        if import_name in file_map:
            target_path = file_map[import_name]
            
            # calculate relative path
            new_rel = os.path.relpath(target_path, file_path.parent)
            new_rel = os.path.splitext(new_rel)[0]
            if not new_rel.startswith('.'):
                new_rel = './' + new_rel
                
            if new_rel != import_path:
                new_content = new_content.replace(f"{prefix}{import_path}{suffix}", f"{prefix}{new_rel}{suffix}")
                changed = True

    if changed:
        file_path.write_text(new_content, encoding='utf-8')
        print(f"Fixed imports in {file_path.name}")

