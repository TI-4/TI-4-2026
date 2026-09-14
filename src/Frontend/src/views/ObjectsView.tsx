import { useState } from 'react';
import { Select } from '../components/Select';
import { Button } from '../components/Button';
import { Input } from '../components/Input';

const SearchIcon = () => (
  <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
  </svg>
);

const BuildingIcon = () => (
  <svg className="w-5 h-5 text-gray-700" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

const SortIcon = () => (
  <svg className="w-5 h-5 text-gray-700" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 4h13M3 8h9m-9 4h6m4 0l4-4m0 0l4 4m-4-4v12" />
  </svg>
);

const CategoryIcon = () => (
  <svg className="w-5 h-5 text-gray-700" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
  </svg>
);

export const ObjectsView = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCampus, setSelectedCampus] = useState('san-juan-pablo-ii');
  const [sortOrder, setSortOrder] = useState('reciente');
  const [selectedCategory, setSelectedCategory] = useState('todos');

  const campusOptions = [
    { value: 'san-juan-pablo-ii', label: 'Campus San Juan Pablo II' },
    { value: 'san-francisco', label: 'Campus San Francisco' },
    { value: 'menchaca-lira', label: 'Campus Menchaca Lira' },
  ];

  const sortOptions = [
    { value: 'reciente', label: 'Reciente' },
    { value: 'antiguo', label: 'Antiguo' },
  ];

  const categoryOptions = [
    { value: 'todos', label: 'Todos los objetos' },
    { value: 'documentos', label: 'Documentos' },
    { value: 'electronica', label: 'Electrónica' },
    { value: 'llaves', label: 'Llaves' },
    { value: 'ropa', label: 'Ropa' },
    { value: 'accesorios', label: 'Accesorios' },
    { value: 'otros', label: 'Otros' },
  ];

  const handleCreateReport = () => {
    console.log('Crear reporte de objeto cliqueado');
  };

  return (
    <div className="p-6 min-h-screen bg-page-dark flex flex-col gap-6">
      <div className="bg-page-dark py-2 px-4 flex flex-col gap-4">
        <div className="flex flex-wrap items-end justify-end gap-4">
          <div className="w-80">
            <Input
              label="Búsqueda"
              placeholder="Buscar Pérdida..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={<SearchIcon />}
            />
          </div>

          <div className="w-72">
            <Select
              label="Campus"
              options={campusOptions}
              value={selectedCampus}
              onChange={(val) => setSelectedCampus(val)}
              icon={<BuildingIcon />}
            />
          </div>

          <div>
            <Button
              variant="solid"
              color="blue"
              size="md"
              onClick={handleCreateReport}
              className="font-bold tracking-wide shadow-md hover:scale-105 transition-transform whitespace-nowrap !w-auto px-6 h-[50px] flex items-center justify-center"
            >
              + CREAR REPORTE
            </Button>
          </div>
        </div>

        <div className="flex flex-wrap items-end justify-end gap-4 pt-2">
          <div className="w-52">
            <Select
              label="Ordenar por"
              options={sortOptions}
              value={sortOrder}
              onChange={(val) => setSortOrder(val)}
              icon={<SortIcon />}
            />
          </div>

          <div className="w-72">
            <Select
              label="Tipo de Objeto"
              options={categoryOptions}
              value={selectedCategory}
              onChange={(val) => setSelectedCategory(val)}
              icon={<CategoryIcon />}
            />
          </div>
        </div>
      </div>

      <div className="flex-1 flex items-center justify-center p-12 text-gray-400">
        <p className="text-lg">Explora o busca objetos perdidos utilizando los filtros superiores.</p>
      </div>
    </div>
  );
};

