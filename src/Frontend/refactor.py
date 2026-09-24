import os
import re
import shutil
from pathlib import Path

# Base directory
src_dir = Path('/home/nacho/Documents/university/uct-map/TI-4-2026/src/Frontend/src').resolve()

# Mappings (old path relative to src_dir -> new path relative to src_dir)
moves = {
    'components/AppLayout.tsx': 'components/layout/AppLayout.tsx',
    'components/Avatar.tsx': 'components/media/Avatar.tsx',
    'components/Button.tsx': 'components/ui/Button.tsx',
    'components/CheckboxItem.tsx': 'components/ui/CheckboxItem.tsx',
    'components/EmptyState.tsx': 'components/ui/EmptyState.tsx',
    'components/ExitButton.tsx': 'components/ui/ExitButton.tsx',
    'components/FileUpload.tsx': 'components/ui/FileUpload.tsx',
    'components/GlobalModals.tsx': 'components/layout/GlobalModals.tsx',
    'components/IconText.tsx': 'components/ui/IconText.tsx',
    'components/ImageControls.tsx': 'components/media/ImageControls.tsx',
    'components/ImageGallery.tsx': 'components/media/ImageGallery.tsx',
    'components/ImagePagination.tsx': 'components/media/ImagePagination.tsx',
    'components/Input.tsx': 'components/ui/Input.tsx',
    'components/LoadingSpinner.tsx': 'components/ui/LoadingSpinner.tsx',
    'components/MapMarker.tsx': 'modules/map/MapMarker.tsx',
    'components/ModalOverlay.tsx': 'components/layout/ModalOverlay.tsx',
    'components/NavButton.tsx': 'components/ui/NavButton.tsx',
    'components/Panel.tsx': 'components/layout/Panel.tsx',
    'components/PhotoFrame.tsx': 'components/media/PhotoFrame.tsx',
    'components/RoomInfoCard.tsx': 'modules/map/RoomInfoCard.tsx',
    'components/ScheduleCard.tsx': 'modules/map/ScheduleCard.tsx',
    'components/SearchInput.tsx': 'components/ui/SearchInput.tsx',
    'components/SectionButton.tsx': 'components/ui/SectionButton.tsx',
    'components/Select.tsx': 'components/ui/Select.tsx',
    'components/SquareButton.tsx': 'components/ui/SquareButton.tsx',
    'components/Tag.tsx': 'components/ui/Tag.tsx',
    'components/UserActionInfo.tsx': 'components/ui/UserActionInfo.tsx',
    'modules/actions/TimedActionCard.tsx': 'components/ui/TimedActionCard.tsx'
}

# Resolve to absolute paths
abs_moves = {src_dir / old: src_dir / new for old, new in moves.items()}

# 1. Move files
for old_path, new_path in abs_moves.items():
    if old_path.exists():
        new_path.parent.mkdir(parents=True, exist_ok=True)
        shutil.move(str(old_path), str(new_path))
        print(f"Moved {old_path.name} to {new_path.parent.name}")

# Remove empty actions directory
actions_dir = src_dir / 'modules/actions'
if actions_dir.exists() and not any(actions_dir.iterdir()):
    actions_dir.rmdir()

# 2. Update imports
# Regex to match imports: import ... from '...'; or import '...';
import_regex = re.compile(r"(from\s+['\"]|import\s+['\"])(.*?)(['\"])")

def calculate_new_import(current_file_path, imported_path_str):
    # imported_path_str is like '../../components/Button'
    if not imported_path_str.startswith('.'):
        return imported_path_str # Ignore absolute or node_modules imports

    # Resolve what file it pointed to
    # Add .tsx or .ts to resolve it
    target_path = (current_file_path.parent / imported_path_str).resolve()
    
    # Check if this target_path matches any old path
    for old_path, new_path in abs_moves.items():
        # Check without extension since imports don't have .tsx
        if target_path.with_suffix('.tsx') == old_path or target_path.with_suffix('.ts') == old_path:
            # We found a match! Calculate new relative path
            new_rel = os.path.relpath(new_path, current_file_path.parent)
            # Remove the .tsx extension
            new_rel = os.path.splitext(new_rel)[0]
            if not new_rel.startswith('.'):
                new_rel = './' + new_rel
            return new_rel
    
    # Check if the current file itself was moved! If it was moved, its relative imports to UNMOVED files must be updated.
    return None

for file_path in src_dir.rglob('*.*'):
    if file_path.suffix not in ['.ts', '.tsx'] or 'assets' in file_path.parts:
        continue

    content = file_path.read_text(encoding='utf-8')
    new_content = content
    changed = False

    # Find all imported paths
    for match in import_regex.finditer(content):
        prefix, import_path, suffix = match.groups()
        
        # Scenario A: The file being imported was moved
        new_import_path = calculate_new_import(file_path, import_path)
        if new_import_path:
            new_content = new_content.replace(f"{prefix}{import_path}{suffix}", f"{prefix}{new_import_path}{suffix}")
            changed = True
            continue

        # Scenario B: The file reading the import was moved, and the import is relative, so the relative path must change.
        if import_path.startswith('.'):
            # Check if current file is in new_paths
            # Where did this file come from?
            old_current_file = None
            for old_p, new_p in abs_moves.items():
                if file_path == new_p:
                    old_current_file = old_p
                    break
            
            if old_current_file:
                # The file was moved. We need to resolve the import from its OLD location
                target_path = (old_current_file.parent / import_path).resolve()
                # Now calculate the new relative path from its NEW location
                new_rel = os.path.relpath(target_path, file_path.parent)
                if not new_rel.startswith('.'):
                    new_rel = './' + new_rel
                if new_rel != import_path:
                    new_content = new_content.replace(f"{prefix}{import_path}{suffix}", f"{prefix}{new_rel}{suffix}")
                    changed = True

    if changed:
        file_path.write_text(new_content, encoding='utf-8')
        print(f"Updated imports in {file_path.name}")

print("Refactor complete.")
