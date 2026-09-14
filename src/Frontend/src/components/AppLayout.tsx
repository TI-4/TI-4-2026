import { Outlet, useLocation } from 'react-router-dom';
import { SectionButton } from './SectionButton';
import React, { useState } from 'react';

import logoUrl from '../assets/svg/logo.svg';
import MapIcon from '../assets/svg/icon_section_map.svg?react';
import ReportsIcon from '../assets/svg/icon_section_incidents.svg?react';
import ObjectsIcon from '../assets/svg/icon_section_objects.svg?react';
import ContactsIcon from '../assets/svg/icon_section_contacts.svg?react';

export const AppLayout = () => {
  const [isExpanded, setIsExpanded] = useState(false);
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
    <div className="flex h-screen bg-gray-50 overflow-hidden font-sans">
      {/* NAVEGATOR */}
      <nav
        className={`bg-page-blue flex flex-col gap-4 py-8 px-4 h-full shadow-xl transition-all duration-300 z-50 ${
          isExpanded ? 'w-[264px]' : 'w-[88px]'
        }`}
        onMouseEnter={() => setIsExpanded(true)}
        onMouseLeave={() => setIsExpanded(false)}
      >
        {/* LOGO CONTAINER */}
        <div
          className={`flex items-center mb-4 bg-white rounded-2xl relative transition-all duration-300 ease-in-out shadow-md flex-shrink-0 z-50 w-max overflow-hidden ${
            isExpanded ? 'h-24 min-w-[232px] pr-2' : 'h-14 min-w-0 pr-4'
          }`}
        >
          {/* LOGO */}
          <div className={`flex-shrink-0 flex items-center justify-center z-10 transition-all duration-300 ease-in-out ${isExpanded ? 'w-24 h-24' : 'w-14 h-14'}`}>
            <img
              src={logoUrl}
              alt="UCT Logo"
              className={`object-contain transition-all duration-300 ease-in-out ${isExpanded ? 'w-20 h-20' : 'w-15 h-15'}`}
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
          expandedWidth="232px"
          icon={<MapIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/reports"
          label="Reportes"
          isExpanded={isExpanded}
          expandedWidth="232px"
          icon={<ReportsIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/objects"
          label="Objetos"
          isExpanded={isExpanded}
          expandedWidth="232px"
          icon={<ContactsIcon className="w-8 h-8" />}
        />

        <SectionButton
          to="/contacts"
          label="Contactos"
          isExpanded={isExpanded}
          expandedWidth="232px"
          icon={<ObjectsIcon className="w-8 h-8" />}
        />
      </nav>

      <main className="flex-1 relative overflow-auto bg-gray-50">
        <Outlet />
      </main>
    </div>
  );
};
