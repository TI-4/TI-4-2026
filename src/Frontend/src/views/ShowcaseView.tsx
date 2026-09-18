import { useState } from 'react';
import { Button } from '../components/Button';
import { Input } from '../components/Input';
import { Select } from '../components/Select';
import { NavButton } from '../components/NavButton';
import { CheckboxItem } from '../components/CheckboxItem';
import { Tag } from '../components/Tag';
import { Panel } from '../components/Panel';
import { MapMarker } from '../components/MapMarker';
import { IconText } from '../components/IconText';
import { PhotoFrame } from '../components/PhotoFrame';
import { Avatar } from '../components/Avatar';
import { ImageGallery } from '../components/ImageGallery';
import { FileUpload } from '../components/FileUpload';
import { SquareButton } from '../components/SquareButton';
import { SectionButton } from '../components/SectionButton';
import { UserActionInfo } from '../components/UserActionInfo';
import { ContactProfilePanel } from '../modules/contacts/ContactProfilePanel';
import { ContactCard } from '../modules/contacts/ContactCard';
import { ObjectReportCard } from '../modules/objects/ObjectReportCard';
import { IncidentCard } from '../modules/incidents/IncidentCard';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { UCT_LOADING_PHRASES } from '../constants/loadingPhrases';
import { RoomInfoCard } from '../components/RoomInfoCard';
import { ScheduleCard } from '../components/ScheduleCard';
import { MapFiltersPanel } from '../modules/map/MapFiltersPanel';
import { BuildingDetailCard } from '../modules/map/BuildingDetailCard';
import { PublishReportCard } from '../modules/objects/PublishReportCard';
import type { FilterSection } from '../interfaces/FilterSection';

const BuildingIcon = () => (
  <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

export const ShowcaseView = () => {
  // Estado mock para los filtros del showcase
  const [filters, setFilters] = useState<FilterSection[]>([
    {
      id: 'academico',
      title: 'ACADÉMICO',
      items: [
        { id: 'edificios', label: 'Edificios', icon: <BuildingIcon />, checked: true },
        { id: 'accesos', label: 'Accesos', icon: <BuildingIcon />, checked: true },
        { id: 'biblioteca', label: 'Biblioteca', icon: <BuildingIcon />, checked: false },
      ]
    },
    {
      id: 'servicios',
      title: 'SERVICIOS',
      items: [
        { id: 'primeros-auxilios', label: 'Primeros Auxilios', icon: <BuildingIcon />, checked: true },
        { id: 'cajeros', label: 'Cajeros', icon: <BuildingIcon />, checked: true },
      ]
    },
    {
      id: 'vida-universitaria',
      title: 'VIDA UNIVERSITARIA',
      items: [
        { id: 'casino', label: 'Casino', icon: <BuildingIcon />, checked: true },
        { id: 'canchas', label: 'Canchas', icon: <BuildingIcon />, checked: false },
        { id: 'kioskos', label: 'Kioskos', icon: <BuildingIcon />, checked: false },
        { id: 'areas-verdes', label: 'Áreas Verdes', icon: <BuildingIcon />, checked: true },
      ]
    }
  ]);

  const handleToggleFilter = (sectionId: string, itemId: string) => {
    setFilters(prev => prev.map(sec => 
      sec.id === sectionId 
        ? { ...sec, items: sec.items.map(item => item.id === itemId ? { ...item, checked: !item.checked } : item) }
        : sec
    ));
  };

  return (
    <div className="min-h-screen bg-gray-50 p-12 flex flex-col gap-8 items-center font-sans">

      <MapFiltersPanel 
        title="Filtros (2)" 
        sections={filters} 
        onToggleItem={handleToggleFilter} 
      />

      <ContactProfilePanel
        name="NOMBRE PROFESOR"
        degree="Grado Académico"
        role="Cargo Principal"
        department="Facultad y Departamento"
        office="Oficina Física"
        email="Correo@uct.cl"
        phone="Número contacto"
      />

      <ContactCard
        name="NOMBRE CONTACTO"
        role="Cargo Principal"
        department="Facultad y Departamento"
      />

      <ObjectReportCard
        title="OBJETO"
        status="Publicado"
        statusColor="blue"
        building="EDIFICIO 11"
        code="OP-2026-0001"
        actions={[
          { title: "Reportado por Usuario", subtitle: "Hace 24 minutos", avatarSrc: "https://i.pravatar.cc/150?img=11" },
          { title: "Recuperado por Usuario", subtitle: "Hace 12 minutos", avatarSrc: "https://i.pravatar.cc/150?img=11" }
        ]}
      />

      <IncidentCard
        location="EDIFICIO 11"
        status="SIN REVISAR"
        statusColor="red"
        photoUrl="https://images.unsplash.com/photo-1584622650111-993a426fbf0a?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
        title="Baño roto"
        reporterName="Juan Pérez"
        reportTime="Hace 24 minutos"
        reporterAvatar="https://i.pravatar.cc/150?img=11"
        description="Estudiantes destruyeron un baño hace unos minutos en el edificio 8..."
      />

      {/* Sección Showcase de Variantes del LoadingSpinner */}
      <Panel color="white" innerClassName="p-8 flex flex-col gap-6 items-center w-full max-w-4xl">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8 items-center justify-items-center w-full">
          {/* Classic Variant */}
          <div className="flex flex-col items-center gap-3 p-4 bg-gray-50 rounded-xl border border-gray-100 w-full min-h-[220px] justify-center">
            <span className="text-xs font-bold text-gray-500 uppercase tracking-wider">Classic</span>
            <LoadingSpinner size="lg" texts={UCT_LOADING_PHRASES} variant="classic" />
          </div>

          {/* Dark Variant */}
          <div className="flex flex-col items-center gap-3 p-4 bg-gray-100 rounded-xl border border-gray-200 w-full min-h-[220px] justify-center">
            <span className="text-xs font-bold text-gray-700 uppercase tracking-wider">Dark / Black</span>
            <LoadingSpinner size="lg" texts={UCT_LOADING_PHRASES} variant="dark" />
          </div>

          {/* Light Variant (on dark background container) */}
          <div className="flex flex-col items-center gap-3 p-4 bg-gray-900 rounded-xl border border-gray-800 w-full min-h-[220px] justify-center">
            <span className="text-xs font-bold text-gray-300 uppercase tracking-wider">Light / White</span>
            <LoadingSpinner size="lg" texts={UCT_LOADING_PHRASES} variant="light" />
          </div>

          {/* Sin Texto (hideText) */}
          <div className="flex flex-col items-center gap-3 p-4 bg-gray-50 rounded-xl border border-gray-100 w-full min-h-[220px] justify-center">
            <span className="text-xs font-bold text-gray-500 uppercase tracking-wider">Sin Texto</span>
            <LoadingSpinner size="lg" variant="classic" hideText />
          </div>
        </div>
      </Panel>

      <Panel color="white" innerClassName="p-6 flex flex-wrap gap-4 items-center">
        <Button variant="solid">Guardar</Button>
        <Button variant="outline">Cancelar</Button>
        <Button variant="ghost">Ignorar</Button>
      </Panel>

      <Panel color="white" innerClassName="p-6 flex flex-col gap-4 min-w-[350px]">
        <Input label="Normal" placeholder="Placeholder" />
        <Input label="Con Error" placeholder="Placeholder" error="Mensaje de error" />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex flex-col gap-4 min-w-[350px]">
        <Select label="Normal" options={[{ value: '1', label: 'Opción 1' }]} />
        <Select label="Con Error" error="Selecciona una opción" options={[{ value: '1', label: 'Opción 1' }]} />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-4 items-center">
        <NavButton to="/" label="Activo" />
        <NavButton to="/inactivo" label="Inactivo" />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-4 items-center">
        <SectionButton to="/" label="Mapa" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M14 6l-3.75 5 2.85 3.8-1.6 1.2C9.81 13.75 7 10 7 10l-6 8h22L14 6z"/></svg>} />
        <SectionButton to="/reportes" label="Reportes" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>} />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-4 items-center">
        <SquareButton>
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M14 6l-3.75 5 2.85 3.8-1.6 1.2C9.81 13.75 7 10 7 10l-6 8h22L14 6z"/></svg>
        </SquareButton>
      </Panel>

      <Panel color="white" innerClassName="p-6 flex flex-col gap-4 min-w-[350px]">
        <CheckboxItem label="Opción 1" />
        <CheckboxItem label="Opción 2" />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-4 flex-wrap">
        <Tag label="Mantenimiento" color="yellow" />
        <Tag label="Cerrado" color="red" />
        <Tag label="Abierto" color="green" />
        <Tag label="Edificio" color="blue" />
      </Panel>

      <Panel color="white" innerClassName="p-8 flex flex-wrap gap-8 bg-gray-200 justify-center">
        <Panel withUctBorder innerClassName="p-4 w-56 h-56 flex items-center justify-center font-bold text-lg text-gray-700">Principal</Panel>
        <Panel color="white" innerClassName="p-4 w-56 h-56 flex items-center justify-center font-bold text-lg text-gray-700">Blanco</Panel>
        <Panel color="gray" innerClassName="p-4 w-56 h-56 flex items-center justify-center font-bold text-lg text-gray-700">Gris</Panel>
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-8 bg-gray-200 justify-center">
        <MapMarker color="blue" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor" stroke="none"><rect x="4" y="2" width="16" height="20" rx="2" ry="2"></rect><path d="M9 22v-4h6v4"></path></svg>} />
        <MapMarker color="red" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>} />
        <MapMarker color="yellow" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z"/><path d="M12 9v4"/><path d="M12 17h.01"/></svg>} />
        <MapMarker color="green" icon={<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M20 6 9 17l-5-5"/></svg>} />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex flex-col gap-4">
        <IconText text="Contacto" icon={<svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24" fill="currentColor"><path d="M6.62 10.79c1.44 2.83 3.76 5.14 6.59 6.59l2.2-2.2c.27-.27.67-.36 1.02-.24 1.12.37 2.33.57 3.57.57.55 0 1 .45 1 1V20c0 .55-.45 1-1 1-9.39 0-17-7.61-17-17 0-.55.45-1 1-1h3.5c.55 0 1 .45 1 1 0 1.25.2 2.45.57 3.57.11.35.03.74-.25 1.02l-2.2 2.2z"/></svg>} />
        <IconText text="Correo" icon={<svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24" fill="currentColor"><path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z"/></svg>} />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-8 items-center">
        <PhotoFrame className="w-24 h-24" />
        <PhotoFrame className="w-24 h-24" src="https://images.unsplash.com/photo-1541339907198-e08756dedf3f?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80" />
      </Panel>

      <Panel color="white" innerClassName="p-6 flex gap-8 items-center">
        <Avatar />
        <Avatar src="https://images.unsplash.com/photo-1599566150163-29194dcaad36?ixlib=rb-4.0.3&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80" />
      </Panel>

      <Panel color="white" innerClassName="p-6">
        <UserActionInfo title="Recuperado por Usuario" subtitle="Hace 12 minutos" avatarSrc="https://i.pravatar.cc/150?img=11" />
      </Panel>

      <RoomInfoCard title="SALA 11-101" type="Sala de clases" capacity={60} />

      <ScheduleCard schedule="08:00 - 22:00 hrs" className="max-w-xs" />

      <Panel color="white" innerClassName="p-6">
        <FileUpload />
      </Panel>

      <Panel color="white" innerClassName="p-6">
        <ImageGallery
          className="w-[300px] h-[150px]"
          images={[
            "https://images.unsplash.com/photo-1541339907198-e08756dedf3f?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80"
          ]}
        />
      </Panel>

      <PublishReportCard />

      <BuildingDetailCard />

    </div>
  );
};
