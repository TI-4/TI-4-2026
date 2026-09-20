import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { UserActionInfo } from '../../components/UserActionInfo';
import { IconText } from '../../components/IconText';
import { Tag } from '../../components/Tag';
import MapPinIcon from '../../assets/svg/icons/icon_location.svg?react';


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
        <IconText icon={<MapPinIcon className="w-8 h-8" />} text={location} className="!text-[26px]" />
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
