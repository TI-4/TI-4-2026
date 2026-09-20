import { useState } from 'react';
import { Select } from '../components/Select';
import { Input } from '../components/Input';

import SearchIcon from '../assets/svg/icons/icon_search.svg?react';
import RoleIcon from '../assets/svg/icons/icon_role.svg?react';

export const ContactsView = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedRole, setSelectedRole] = useState('todos');

  const roleOptions = [
    { value: 'todos', label: 'Todos los roles' },
    { value: 'profesores', label: 'Profesores' },
    { value: 'directiva', label: 'Directiva' },
    { value: 'administrativos', label: 'Administrativos' },
    { value: 'estudiantes', label: 'Estudiantes' },
  ];

  return (
    <div className="p-6 min-h-screen bg-page-dark flex flex-col gap-6">
      <div className="bg-page-dark py-2 px-4 flex flex-wrap items-end justify-end gap-4">
        <div className="w-80">
          <Input
            label="Búsqueda"
            placeholder="Buscar Contacto..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            icon={<SearchIcon className="w-5 h-5 text-gray-500" />}
          />
        </div>

        <div className="w-72">
          <Select
            label="Rol"
            options={roleOptions}
            value={selectedRole}
            onChange={(val) => setSelectedRole(val)}
            icon={<RoleIcon className="w-5 h-5 text-gray-700" />}
          />
        </div>
      </div>

      <div className="flex-1 flex items-center justify-center p-12 text-gray-400">
        <p className="text-lg">Busca contactos institucionales o filtra por rol utilizando los controles superiores.</p>
      </div>
    </div>
  );
};

