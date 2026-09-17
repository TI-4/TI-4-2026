import React from 'react';
import { Panel } from '../../components/Panel';
import { CloseButton } from '../../components/ExitButton';
import { ImageGallery } from '../../components/ImageGallery';
import { Tag } from '../../components/Tag';
import { RoomInfoCard } from '../../components/RoomInfoCard';
import { ScheduleCard } from '../../components/ScheduleCard';
import { Button } from '../../components/Button';

const BuildingIcon = () => (
  <svg className="w-8 h-8 text-gray-800" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

const RecycleIcon = () => (
  <svg className="w-5 h-5 text-white" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2l-4 7h3v5h2V9h3l-4-7zm-7 8l-3 5.2 2.6 1.5L6.4 13h5.6v-2H6.4l-1.4-1.2zM19 10l-1.4 1.2H12v2h5.6l1.8 3.7 2.6-1.5L19 10zm-1.6 9H6.6l-1 2h12.8l-1-2z" />
  </svg>
);

const WarningIcon = () => (
  <svg className="w-6 h-6 text-gray-800" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M12 9v4m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
  </svg>
);

const NavigationIcon = () => (
  <svg className="w-6 h-6 text-white" fill="currentColor" viewBox="0 0 24 24">
    <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
  </svg>
);

const Rotate360Icon = () => (
  <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M4 4v5h5M20 20v-5h-5M4 9a9 9 0 0115.36-4.36L20 5M4 19l.64-.64A9 9 0 0020 15" />
  </svg>
);

export interface RoomData {
  title: string;
  type: string;
  capacity: number | string;
}

export interface BuildingDetailCardProps {
  title?: string;
  subtitle?: string;
  images?: string[];
  schedule?: string;
  floors?: string;
  services?: string[];
  rooms?: RoomData[];
  onClose?: () => void;
  onNavigate?: () => void;
  onView360?: () => void;
  onReportProblem?: () => void;
}

const defaultImages = [
  "https://images.unsplash.com/photo-1541339907198-e08756dedf3f?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
];

export const BuildingDetailCard = ({
  title = "NOMBRE EDIFICIO",
  subtitle = "EDIFICIO 11",
  images = defaultImages,
  schedule = "08:00 - 22:00 hrs",
  floors = "2 PISOS",
  services = ["Punto de Reciclaje"],
  rooms = [{ title: "SALA 11-101", type: "Sala de clases", capacity: 60 }],
  onClose,
  onNavigate,
  onView360,
  onReportProblem,
}: BuildingDetailCardProps) => {
  return (
    <Panel withUctBorder color="white" outerClassName="rounded-3xl max-w-sm w-full" innerClassName="p-5 flex flex-col gap-4">
      <div className="flex items-start justify-between gap-2">
        <div className="flex items-center gap-3">
          <BuildingIcon />
          <div className="flex flex-col">
            <h2 className="text-xl font-extrabold text-gray-800 leading-tight uppercase tracking-tight">
              {title}
            </h2>
            <span className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
              {subtitle}{floors ? ` - ${floors}` : ''}
            </span>
          </div>
        </div>
        <CloseButton variant="ghost" size="lg" onClick={onClose} />
      </div>

      <ImageGallery
        className="w-full h-44 rounded-2xl overflow-hidden"
        images={images}
      />

      <ScheduleCard schedule={schedule} />

      <div className="flex flex-col gap-1.5">
        <span className="text-xs font-bold text-gray-600 uppercase tracking-wider">
          SERVICIOS
        </span>
        <div className="flex flex-wrap gap-2">
          {services.map((srv, idx) => (
            <Tag key={idx} label={srv} color="blue" icon={<RecycleIcon />} className="text-xs py-1.5 px-3.5" />
          ))}
        </div>
      </div>

      <div className="flex flex-col gap-2">
        <span className="text-xs font-bold text-gray-600 uppercase tracking-wider">
          SALAS
        </span>
        <div className="flex flex-col gap-2 max-h-48 overflow-y-auto pr-1">
          {rooms.map((room, idx) => (
            <RoomInfoCard
              key={idx}
              title={room.title}
              type={room.type}
              capacity={room.capacity}
              className="max-w-none text-sm p-3"
            />
          ))}
        </div>
      </div>

      <button
        type="button"
        onClick={onReportProblem}
        className="flex items-center gap-3 py-2 text-gray-800 hover:text-red-600 transition-colors font-bold text-sm text-left group"
      >
        <WarningIcon />
        <span className="underline group-hover:no-underline">Reportar un problema aquí</span>
      </button>

      <div className="border-t border-gray-200 pt-3 flex flex-col gap-2.5">
        <Button
          color="blue"
          variant="solid"
          onClick={onNavigate}
          className="w-full font-black text-base flex items-center justify-center gap-2 py-3 tracking-wider uppercase rounded-xl"
        >
          <NavigationIcon />
          <span>LLEGAR</span>
        </Button>

        <Button
          color="blue"
          variant="solid"
          onClick={onView360}
          className="w-full font-black text-base flex items-center justify-center gap-2 py-3 tracking-wider uppercase rounded-xl"
        >
          <Rotate360Icon />
          <span>360°</span>
        </Button>
      </div>
    </Panel>
  );
};
