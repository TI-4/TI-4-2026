import { useState } from 'react';
import { SearchInput } from '../components/SearchInput';
import { EmptyState } from '../components/EmptyState';

import SearchIcon from '../assets/svg/icons/icon_search.svg?react';

export const UsersAdminView = () => {
  const [searchQuery, setSearchQuery] = useState('');

  const userSearchOptions = [
    { value: 'admin', label: 'Administrador' },
    { value: 'juan-perez', label: 'Juan Pérez' }
  ];

  return (
    <div className="p-6 min-h-screen bg-page-dark flex flex-col gap-6">
      <div className="bg-page-dark py-2 px-4 flex flex-wrap items-end justify-end gap-4">
        <div className="w-80">
          <SearchInput
            label="Búsqueda de Usuarios"
            placeholder="Buscar por RUT, Nombre o Correo..."
            value={searchQuery}
            onChange={(val) => setSearchQuery(val)}
            icon={<SearchIcon className="w-5 h-5 text-gray-500" />}
            options={userSearchOptions}
          />
        </div>
      </div>

      <div className="flex-1 flex items-center justify-center p-12">
        <EmptyState message="Utiliza el buscador superior para encontrar usuarios." />
      </div>
    </div>
  );
};
