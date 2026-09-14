import React from 'react';

const DoorIcon = () => (
  <svg className="w-10 h-10 text-gray-900" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M3 21h18M5 21V5a2 2 0 012-2h10a2 2 0 012 2v16M15 12h.01" />
  </svg>
);

const UserIcon = () => (
  <svg className="w-5 h-5 text-gray-900" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z" />
  </svg>
);

interface RoomInfoCardProps {
  title: string;
  type: string;
  capacity: number | string;
  icon?: React.ReactNode;
  className?: string;
}

export const RoomInfoCard = ({
  title,
  type,
  capacity,
  icon,
  className = '',
}: RoomInfoCardProps) => {
  return (
    <div className={`bg-[#e0e0e0] rounded-2xl px-5 py-4 flex items-center justify-between gap-6 shadow-sm w-full max-w-lg ${className}`}>
      <div className="flex items-center gap-4">
        <div className="flex-shrink-0 flex items-center justify-center">
          {icon || <DoorIcon />}
        </div>
        <div className="flex flex-col justify-center">
          <h3 className="text-xl font-bold text-gray-800 tracking-wide uppercase leading-tight">
            {title}
          </h3>
          <p className="text-base font-semibold text-gray-700 leading-tight">
            {type}
          </p>
        </div>
      </div>

      <div className="flex-shrink-0 bg-gray-400/60 px-3.5 py-1.5 rounded-xl flex items-center gap-2 text-gray-800 font-bold text-base">
        <UserIcon />
        <span>{capacity}</span>
      </div>
    </div>
  );
};
