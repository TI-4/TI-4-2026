import React from 'react';
import { type PageColor, bgPageColors } from '../constants/colors';

const DoorIcon = () => (
  <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2">
    <path strokeLinecap="round" strokeLinejoin="round" d="M3 21h18M5 21V5a2 2 0 012-2h10a2 2 0 012 2v16M15 12h.01" />
  </svg>
);

interface ScheduleCardProps {
  title?: string;
  schedule: string;
  icon?: React.ReactNode;
  color?: PageColor;
  className?: string;
}

export const ScheduleCard = ({
  title = 'Horario de atención',
  schedule,
  icon,
  color = 'blue',
  className = '',
}: ScheduleCardProps) => {
  const bgClass = bgPageColors[color] || bgPageColors.blue;

  return (
    <div className={`${bgClass} text-white rounded-2xl px-4 py-3 flex items-center gap-3.5 shadow-sm w-full ${className}`}>
      <div className="w-9 h-9 border-2 border-white rounded-xl flex items-center justify-center flex-shrink-0">
        {icon || <DoorIcon />}
      </div>
      <div className="flex flex-col justify-center">
        <span className="text-sm font-bold leading-tight block">{title}</span>
        <span className="text-sm font-semibold leading-tight block mt-0.5">{schedule}</span>
      </div>
    </div>
  );
};
