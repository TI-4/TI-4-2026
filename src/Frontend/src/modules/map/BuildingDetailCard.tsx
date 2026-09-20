import React from 'react';
import { Panel } from '../../components/Panel';
import { CloseButton } from '../../components/ExitButton';
import { ImageGallery } from '../../components/ImageGallery';
import { Tag } from '../../components/Tag';
import { RoomInfoCard } from '../../components/RoomInfoCard';
import { ScheduleCard } from '../../components/ScheduleCard';
import { Button } from '../../components/Button';
import BuildingIcon from '../../assets/svg/icons/icon_building.svg?react';
import WarningIcon from '../../assets/svg/icons/icon_warning.svg?react';
import NavigationIcon from '../../assets/svg/icons/icon_navigation.svg?react';
import Rotate360Icon from '../../assets/svg/icons/icon_repeat.svg?react';




import type { RoomData } from '../../interfaces/RoomData';

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
          <BuildingIcon className="w-8 h-8 text-gray-800" />
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
            <Tag key={idx} label={srv} color="blue" className="text-xs py-1.5 px-3.5" />
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
        <WarningIcon className="w-6 h-6 text-gray-800" />
        <span className="underline group-hover:no-underline">Reportar un problema aquí</span>
      </button>

      <div className="border-t border-gray-200 pt-3 flex flex-col gap-2.5">
        <Button
          color="blue"
          variant="solid"
          onClick={onNavigate}
          className="w-full font-black text-base flex items-center justify-center gap-2 py-3 tracking-wider uppercase rounded-xl"
        >
          <NavigationIcon className="w-6 h-6 text-white" />
          <span>LLEGAR</span>
        </Button>

        <Button
          color="blue"
          variant="solid"
          onClick={onView360}
          className="w-full font-black text-base flex items-center justify-center gap-2 py-3 tracking-wider uppercase rounded-xl"
        >
          <Rotate360Icon className="w-6 h-6 text-white" />
          <span>360°</span>
        </Button>
      </div>
    </Panel>
  );
};
