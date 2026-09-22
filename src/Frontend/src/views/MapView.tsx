import React from 'react';
import { TransformWrapper, TransformComponent } from 'react-zoom-pan-pinch';
import WorldMap from '../assets/svg/maps/world.svg?react';
import { Select } from '../components/Select';
import { SearchInput } from '../components/SearchInput';
import { SquareButton } from '../components/SquareButton';
import { HeatmapSpot } from '../modules/map/HeatmapSpot';

import SearchIcon from '../assets/svg/icons/icon_search.svg?react';
import BuildingIcon from '../assets/svg/icons/icon_building.svg?react';
import PlusIcon from '../assets/svg/icons/icon_plus.svg?react';
import MinusIcon from '../assets/svg/icons/icon_minus.svg?react';
import TargetIcon from '../assets/svg/icons/icon_target.svg?react';
import PencilIcon from '../assets/svg/icons/icon_pencil.svg?react';
import { RoleGuard } from '../router/RoleGuard';
import { MapEditorPanel } from '../modules/admin/MapEditorPanel';

export const MapView = () => {
  const [searchQuery, setSearchQuery] = React.useState('');
  const [isEditorOpen, setIsEditorOpen] = React.useState(false);

  const mapSearchOptions = [
    { value: 'biblioteca', label: 'Biblioteca Central' },
    { value: 'casino', label: 'Casino' },
    { value: 'edificio-c', label: 'Edificio C' },
    { value: 'gimnasio', label: 'Gimnasio' },
    { value: 'auditorio', label: 'Auditorio Principal' }
  ];
  return (
    <div className="w-full h-full absolute inset-0 overflow-hidden">
      <div className="absolute top-6 right-6 z-10 flex flex-col gap-3 items-end">
        <div className="w-80">
          <SearchInput
            label="Búsqueda"
            placeholder="Buscar Edificio..."
            value={searchQuery}
            onChange={(val) => setSearchQuery(val)}
            icon={<SearchIcon className="w-6 h-6 text-gray-500" />}
            options={mapSearchOptions}
          />
        </div>

        <div className="w-80">
          <Select
            label="Campus"
            options={[
              { value: 'campus1', label: 'Campus San Juan Pablo II' },
              { value: 'campus2', label: 'Campus Norte' }
            ]}
            value="campus1"
            icon={<BuildingIcon className="w-6 h-6 text-gray-700" />}
          />
        </div>
      </div>

      <TransformWrapper
        initialScale={1}
        minScale={0.2}
        maxScale={8}
        centerOnInit={true}
        wheel={{ step: 0.1 }}
      >
        {({ zoomIn, zoomOut, resetTransform }) => (
          <React.Fragment>
            <div className="absolute bottom-6 right-6 z-20 flex items-end gap-6">

              {/* Animate container */}
              <RoleGuard allowedRoles={['ADMIN']}>
                <div
                  className={`transition-all duration-700 ease-[cubic-bezier(0.16,1,0.3,1)] ${
                    isEditorOpen ? 'translate-y-0 opacity-100 pointer-events-auto' : 'translate-y-10 opacity-0 pointer-events-none'
                  }`}
                >
                  <MapEditorPanel />
                </div>
              </RoleGuard>

              {/* Map controls */}
              <div className="flex flex-col gap-2">
                <RoleGuard allowedRoles={['ADMIN']}>
                  <SquareButton
                    onClick={() => setIsEditorOpen(!isEditorOpen)}
                    title="Editar mapa"
                    className={isEditorOpen ? '!bg-page-blue !text-white' : ''}
                  >
                    <PencilIcon className="w-7 h-7" />
                  </SquareButton>
                </RoleGuard>
                <SquareButton onClick={() => zoomIn()} title="Acercar mapa">
                  <PlusIcon className="w-8 h-8" />
                </SquareButton>
                <SquareButton onClick={() => zoomOut()} title="Alejar mapa">
                  <MinusIcon className="w-8 h-8" />
                </SquareButton>
                <SquareButton onClick={() => resetTransform()} title="Centrar mapa">
                  <TargetIcon className="w-7 h-7" />
                </SquareButton>
              </div>
            </div>

            <TransformComponent
              wrapperStyle={{ width: '100%', height: '100%' }}
              contentStyle={{ width: '100%', height: '100%', position: 'relative' }}
              wrapperClass="cursor-grab active:cursor-grabbing"
            >
              <WorldMap
                preserveAspectRatio="xMidYMid slice"
                style={{ width: '100%', height: '100%' }}
                className="pointer-events-auto"
              />

              {/* Mock Heatmap */}
              <HeatmapSpot x="30%" y="40%" intensity={20} />
              <HeatmapSpot x="45%" y="60%" intensity={55} />
              <HeatmapSpot x="60%" y="30%" intensity={85} />
              <HeatmapSpot x="75%" y="70%" intensity={100} />
            </TransformComponent>
          </React.Fragment>
        )}
      </TransformWrapper>
    </div>
  );
};
