import React from 'react';
import { TransformWrapper, TransformComponent } from 'react-zoom-pan-pinch';
import WorldMap from '../assets/svg/maps/world.svg?react';
import { Select } from '../components/Select';
import { Input } from '../components/Input';
import { SquareButton } from '../components/SquareButton';

import SearchIcon from '../assets/svg/icons/icon_search.svg?react';
import BuildingIcon from '../assets/svg/icons/icon_building.svg?react';
import PlusIcon from '../assets/svg/icons/icon_plus.svg?react';
import MinusIcon from '../assets/svg/icons/icon_minus.svg?react';
import CenterIcon from '../assets/svg/icons/icon_center.svg?react';

export const MapView = () => {
  return (
    <div className="w-full h-full absolute inset-0 overflow-hidden">
      <div className="absolute top-6 right-6 z-10 flex flex-col gap-3 items-end">
        <div className="w-80">
          <Input
            label="Búsqueda"
            placeholder="Buscar Edificio..."
            icon={<SearchIcon className="w-6 h-6 text-gray-500" />}
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
            <div className="absolute bottom-6 right-6 z-10 flex flex-col gap-2">
              <SquareButton onClick={() => zoomIn()} title="Acercar mapa">
                <PlusIcon className="w-8 h-8" />
              </SquareButton>
              <SquareButton onClick={() => zoomOut()} title="Alejar mapa">
                <MinusIcon className="w-8 h-8" />
              </SquareButton>
              <SquareButton onClick={() => resetTransform()} title="Centrar mapa">
                <CenterIcon className="w-7 h-7" />
              </SquareButton>
            </div>

            <TransformComponent
              wrapperStyle={{ width: '100%', height: '100%' }}
              contentStyle={{ width: '100%', height: '100%' }}
              wrapperClass="cursor-grab active:cursor-grabbing"
            >
              <WorldMap
                preserveAspectRatio="xMidYMid slice"
                style={{ width: '100%', height: '100%' }}
                className="pointer-events-auto"
              />
            </TransformComponent>
          </React.Fragment>
        )}
      </TransformWrapper>
    </div>
  );
};
