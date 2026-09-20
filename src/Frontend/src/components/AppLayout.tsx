import { Outlet, useLocation } from 'react-router-dom';
import { SectionButton } from './SectionButton';
import { Avatar } from './Avatar';
import { Tag } from './Tag';
import React from 'react';
import { useNavStore } from '../states/navStore';

import logoUrl from '../assets/svg/logo.svg';
import MapIcon from '../assets/svg/icons/icon_section_map.svg?react';
import ReportsIcon from '../assets/svg/icons/icon_section_incidents.svg?react';
import ObjectsIcon from '../assets/svg/icons/icon_section_objects.svg?react';
import ContactsIcon from '../assets/svg/icons/icon_section_contacts.svg?react';

export const AppLayout = () => {
  const { isExpanded, setIsExpanded } = useNavStore();
  const location = useLocation();

  const getPageTitle = (path: string) => {
    if (path.includes('/map')) return 'Mapa';
    if (path.includes('/reports')) return 'Reportes';
    if (path.includes('/objects')) return 'Objetos';
    if (path.includes('/contacts')) return 'Contactos';
    if (path.includes('/showcase')) return 'Showcase';
    return '';
  };

  const pageTitle = getPageTitle(location.pathname);

  return (
    <div className="flex h-screen bg-page-dark overflow-hidden font-sans">
      {/* NAVEGATOR */}
      <nav
        className={`bg-page-blue flex flex-col gap-4 py-8 px-4 h-full shadow-xl transition-all duration-300 z-50 ${
          isExpanded ? 'w-[320px]' : 'w-[88px]'
        }`}
        onMouseEnter={() => setIsExpanded(true)}
        onMouseLeave={() => setIsExpanded(false)}
      >
        {/* LOGO CONTAINER */}
        <div
          className={`flex items-center mb-4 bg-white rounded-2xl relative transition-all duration-300 ease-in-out shadow-md flex-shrink-0 z-50 w-max overflow-hidden ${
            isExpanded ? 'h-24 min-w-[288px] pr-2' : 'h-14 min-w-0 pr-4'
          }`}
        >
          {/* LOGO */}
          <div className={`flex-shrink-0 flex items-center justify-center z-10 transition-all duration-300 ease-in-out ${isExpanded ? 'w-24 h-24' : 'w-14 h-14'}`}>
            <img
              src={logoUrl}
              alt="UCT Logo"
              className={`object-contain transition-all duration-300 ease-in-out ${isExpanded ? 'w-20 h-20' : 'w-10 h-10'}`}
            />
          </div>

          <div className="flex items-center h-full">
            <div
              className={`flex flex-col justify-center transition-all duration-300 ease-in-out overflow-hidden whitespace-nowrap ${
                !isExpanded
                  ? 'max-w-[250px] opacity-100 ml-3'
                  : 'max-w-0 opacity-0 ml-0'
              }`}
            >
              <span className="text-gray-800 font-medium text-2xl">
                {pageTitle}
              </span>
            </div>

            {/* LOGO TEXT */}
            <div
              className={`flex flex-col justify-center transition-all duration-300 ease-in-out overflow-hidden whitespace-nowrap ${
                isExpanded
                  ? 'max-w-[250px] opacity-100 ml-3'
                  : 'max-w-0 opacity-0 ml-0'
              }`}
            >
              <span className="text-gray-800 font-black text-3xl leading-none tracking-wide block">UCT</span>
              <span className="text-gray-800 font-black text-3xl leading-none tracking-wide block">MAP</span>
            </div>
          </div>
        </div>

        <SectionButton
          to="/map"
          label="Mapa"
          isExpanded={isExpanded}
          expandedWidth="288px"
          icon={<MapIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/reports"
          label="Reportes"
          isExpanded={isExpanded}
          expandedWidth="288px"
          icon={<ReportsIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/objects"
          label="Objetos"
          isExpanded={isExpanded}
          expandedWidth="288px"
          icon={<ContactsIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/contacts"
          label="Contactos"
          isExpanded={isExpanded}
          expandedWidth="288px"
          icon={<ObjectsIcon className="w-8 h-8" />}
        />

        {/* User Info Container (At the bottom) */}
        <div className="mt-auto">
          <div
            className={`flex items-center rounded-2xl relative transition-all duration-300 ease-in-out flex-shrink-0 z-50 overflow-hidden ${
              isExpanded ? 'bg-white shadow-md h-24 w-[288px] p-2 pr-4' : 'bg-transparent h-14 w-14 p-0'
            }`}
          >
            {/* Avatar (crece/achica según isExpanded) */}
            <div className={`flex-shrink-0 flex items-center justify-center z-10 transition-all duration-300 ease-in-out ${isExpanded ? 'w-20 h-20' : 'w-14 h-14'}`}>
              <Avatar className="w-full h-full" />
            </div>

            {/* Textos de usuario */}
            <div
              className={`flex flex-col justify-center transition-all duration-300 ease-in-out overflow-hidden whitespace-normal break-words ${
                isExpanded
                  ? 'max-w-[190px] opacity-100 ml-3'
                  : 'max-w-0 opacity-0 ml-0'
              }`}
            >
              <span className="text-gray-800 font-bold text-lg leading-tight line-clamp-2">NOMBRE APELLIDO</span>
              <Tag label="ESTUDIANTE" color="blue" className="mt-1 w-max scale-90 origin-left" />
            </div>
          </div>
        </div>
      </nav>

      <main className="flex-1 relative overflow-auto bg-page-dark">
        <Outlet />
      </main>
    </div>
  );
};
