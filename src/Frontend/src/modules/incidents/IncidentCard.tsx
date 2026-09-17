import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { UserActionInfo } from '../../components/UserActionInfo';
import { IconText } from '../../components/IconText';
import { Tag } from '../../components/Tag';

const MapPinIcon = () => (
  <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M15 10.5a3 3 0 11-6 0 3 3 0 016 0z" />
    <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1115 0z" />
  </svg>
);

export interface IncidentCardProps {
  location: string;
  status: string;
  statusColor?: 'blue' | 'green' | 'yellow' | 'red' | 'gray' | 'white';
  photoUrl?: string;
  title: string;
  reporterName: string;
  reportTime: string;
  reporterAvatar?: string;
  description: string;
  className?: string;
}

export const IncidentCard = ({
  location,
  status,
  statusColor = 'red',
  photoUrl,
  title,
  reporterName,
  reportTime,
  reporterAvatar,
  description,
  className = ''
}: IncidentCardProps) => {
  return (
    <Panel color="white" outerClassName={`w-full max-w-2xl ${className}`} innerClassName="p-6 flex flex-col gap-4">
      {/* Ubication and state */}
      <div className="flex justify-between items-center mb-2">
        <IconText icon={<MapPinIcon />} text={location} className="!text-[26px]" />
        <Tag label={status} color={statusColor as any} className="!text-lg !px-4 !py-1.5 uppercase" />
      </div>

      {/* Incident image */}
      <PhotoFrame src={photoUrl} className="w-full h-72 rounded-3xl object-cover mb-2" />

      {/* Incident name */}
      <h2 className="text-[38px] font-medium text-gray-800 leading-tight">
        {title}
      </h2>

      {/* Report Information */}
      <UserActionInfo
        title={`Reportado por ${reporterName}`}
        subtitle={reportTime}
        avatarSrc={reporterAvatar}
        className="!p-0 !bg-transparent scale-90 origin-left"
      />

      {/* Description */}
      <p className="text-[22px] text-gray-700 leading-relaxed mt-2">
        "{description}"
      </p>
    </Panel>
  );
};
