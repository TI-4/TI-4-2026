import { useState } from 'react';
import { Select } from '../components/Select';
import { Input } from '../components/Input';

const SearchIcon = () => (
  <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
  </svg>
);

const RoleIcon = () => (
  <svg className="w-5 h-5 text-gray-700" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5 5 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
  </svg>
);

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
            icon={<SearchIcon />}
          />
        </div>

        <div className="w-72">
          <Select
            label="Rol"
            options={roleOptions}
            value={selectedRole}
            onChange={(val) => setSelectedRole(val)}
            icon={<RoleIcon />}
          />
        </div>
      </div>

      <div className="flex-1 flex items-center justify-center p-12 text-gray-400">
        <p className="text-lg">Busca contactos institucionales o filtra por rol utilizando los controles superiores.</p>
      </div>
    </div>
  );
};

