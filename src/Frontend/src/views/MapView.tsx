import React from 'react';
import { TransformWrapper, TransformComponent } from 'react-zoom-pan-pinch';
import WorldMap from '../assets/svg/maps/world.svg?react';
import { Select } from '../components/Select';
import { Input } from '../components/Input';
import { SquareButton } from '../components/SquareButton';

const SearchIcon = () => (
  <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M21 21l-4.35-4.35M16.65 16.65A7.5 7.5 0 1116.65 1.65a7.5 7.5 0 010 15z" />
  </svg>
);

const BuildingIcon = () => (
  <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

const PlusIcon = () => (
  <svg className="w-8 h-8" fill="none" stroke="currentColor" strokeWidth="2.5" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
  </svg>
);

const MinusIcon = () => (
  <svg className="w-8 h-8" fill="none" stroke="currentColor" strokeWidth="2.5" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15" />
  </svg>
);

const CenterIcon = () => (
  <svg className="w-7 h-7" fill="none" stroke="currentColor" strokeWidth="2.5" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" d="M3 12h4m10 0h4M12 3v4m0 10v4m0-11a3 3 0 100 6 3 3 0 000-6z" />
  </svg>
);

export const MapView = () => {
  return (
    <div className="w-full h-full absolute inset-0 overflow-hidden">
      <div className="absolute top-6 right-6 z-10 flex flex-col gap-3 items-end">
        <div className="w-80">
          <Input
            label="Búsqueda"
            placeholder="Buscar Edificio..."
            icon={<SearchIcon />}
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
            icon={<BuildingIcon />}
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
                <PlusIcon />
              </SquareButton>
              <SquareButton onClick={() => zoomOut()} title="Alejar mapa">
                <MinusIcon />
              </SquareButton>
              <SquareButton onClick={() => resetTransform()} title="Centrar mapa">
                <CenterIcon />
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
