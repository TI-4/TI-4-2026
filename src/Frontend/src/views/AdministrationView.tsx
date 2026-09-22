import { useState } from 'react';
import { Select } from '../components/Select';
import { SearchInput } from '../components/SearchInput';
import { EmptyState } from '../components/EmptyState';
import { UserManagementCard } from '../modules/admin/UserManagementCard';

import SearchIcon from '../assets/svg/icons/icon_search.svg?react';
import CategoryIcon from '../assets/svg/icons/icon_category_cube.svg?react'; // We can use this as a temporary role icon

export const AdministrationView = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedRole, setSelectedRole] = useState('todos');

  // Role filter options
  const roleOptions = [
    { value: 'todos', label: 'Todos los roles' },
    { value: 'MEMBER', label: 'Miembro' },
    { value: 'TEACHER', label: 'Profesor' },
    { value: 'OFFICIAL', label: 'Funcionario' },
    { value: 'ADMIN', label: 'Administrador' },
    { value: 'INCIDENTS_OFFICER', label: 'Encargado de Incidentes' },
    { value: 'OBJECTS_OFFICER', label: 'Encargado de Objetos' },
  ];

  // Search suggestions (Optional, can come from backend later)
  const userSearchOptions = [
    { value: '11111111-1', label: 'Juan Pérez (11.111.111-1)' },
    { value: '22222222-2', label: 'María González (22.222.222-2)' },
  ];

  return (
    <div className="p-6 min-h-screen bg-page-dark flex flex-col gap-6">

      {/* Filter Bar (right-aligned like in other views) */}
      <div className="bg-page-dark py-2 px-4 flex flex-col gap-4">
        <div className="flex flex-wrap items-end justify-end gap-4">

          {/* User Search */}
          <div className="w-80">
            <SearchInput
              label="Buscar Usuario"
              placeholder="Buscar por Nombre o RUT..."
              value={searchQuery}
              onChange={(val) => setSearchQuery(val)}
              icon={<SearchIcon className="w-5 h-5 text-gray-500" />}
              options={userSearchOptions}
            />
          </div>

          {/* Role Filter */}
          <div className="w-72">
            <Select
              label="Filtrar por Rol"
              options={roleOptions}
              value={selectedRole}
              onChange={(val) => setSelectedRole(val)}
              icon={<CategoryIcon className="w-5 h-5 text-gray-700" />}
            />
          </div>

        </div>
      </div>

      {/* Results Area */}
      <div className="flex-1 flex flex-col gap-4 max-w-5xl w-full mx-auto">

      </div>

    </div>
  );
};
