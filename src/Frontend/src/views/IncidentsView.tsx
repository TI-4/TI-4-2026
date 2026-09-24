import { useState } from 'react';
import { Select } from '../components/ui/Select';
import { Button } from '../components/ui/Button';

import BuildingIcon from '../assets/svg/icons/icon_building.svg?react';
import SortIcon from '../assets/svg/icons/icon_sort.svg?react';
import PlusIcon from '../assets/svg/icons/icon_plus.svg?react';
import { EmptyState } from '../components/ui/EmptyState';

export const IncidentsView = () => {
  const [sortOrder, setSortOrder] = useState('reciente');
  const [selectedCampus, setSelectedCampus] = useState('san-juan-pablo-ii');

  const sortOptions = [
    { value: 'reciente', label: 'Reciente' },
    { value: 'antiguo', label: 'Antiguo' },
  ];

  const campusOptions = [
    { value: 'san-juan-pablo-ii', label: 'Campus San Juan Pablo II' },
    { value: 'san-francisco', label: 'Campus San Francisco' },
    { value: 'menchaca-lira', label: 'Campus Menchaca Lira' },
  ];

  const handleCreateReport = () => {
    console.log('Crear reporte cliqueado');
  };

  return (
    <div className="p-6 min-h-screen bg-page-dark flex flex-col gap-6">
      <div className="bg-page-dark py-2 px-4 flex flex-wrap items-end justify-end gap-4">
        <div className="w-52">
          <Select
            label="Ordenar por"
            options={sortOptions}
            value={sortOrder}
            onChange={(val) => setSortOrder(val)}
            icon={<SortIcon className="w-5 h-5 text-gray-700" />}
          />
        </div>

        <div className="w-72">
          <Select
            label="Campus"
            options={campusOptions}
            value={selectedCampus}
            onChange={(val) => setSelectedCampus(val)}
            icon={<BuildingIcon className="w-5 h-5 text-gray-700" />}
          />
        </div>

        <div>
          <Button
            variant="solid"
            color="blue"
            size="md"
            onClick={handleCreateReport}
            className="font-bold tracking-wide shadow-md hover:scale-105 transition-transform whitespace-nowrap !w-auto px-6 h-[50px] flex items-center justify-center gap-2"
          >
            <PlusIcon className="w-5 h-5 stroke-[3]" /> CREAR REPORTE
          </Button>
        </div>
      </div>

      <div className="flex-1 flex items-center justify-center p-12">
        <EmptyState message="Selecciona los filtros arriba para visualizar los reportes e incidentes." />
      </div>
    </div>
  );
};


