import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { UserActionInfo } from '../../components/UserActionInfo';
import { IconText } from '../../components/IconText';
import { Tag } from '../../components/Tag';
import MapPinIcon from '../../assets/svg/icons/icon_location.svg?react';
import TicketIcon from '../../assets/svg/icons/icon_category.svg?react';


import type { ObjectReportAction } from '../../interfaces/ObjectReportAction';
export interface ObjectReportCardProps {
  title: string;
  status: string;
  statusColor?: 'blue' | 'green' | 'yellow' | 'red' | 'gray' | 'white';
  photoUrl?: string;
  building: string;
  code: string;
  actions: ObjectReportAction[];
  className?: string;
}

export const ObjectReportCard = ({
  title,
  status,
  statusColor = 'blue',
  photoUrl,
  building,
  code,
  actions,
  className = ''
}: ObjectReportCardProps) => {
  return (
    <Panel color="white" outerClassName={`w-full max-w-4xl ${className}`} innerClassName="p-8 flex flex-row gap-8">
      {/* Object Image */}
      <PhotoFrame src={photoUrl} className="w-64 h-64 rounded-2xl flex-shrink-0" />

      {/* Right container */}
      <div className="flex flex-col flex-1 gap-2">
        {/* Title and state */}
        <div className="flex justify-between items-center mb-2">
          <h2 className="text-[42px] font-medium text-gray-800 uppercase tracking-wide leading-none">{title}</h2>
          <Tag label={status} color={statusColor as any} className="!text-[22px] !px-6 !py-2 !font-normal" />
        </div>

        {/* User actions */}
        <div className="flex flex-col gap-2">
          {actions.map((action, idx) => (
            <UserActionInfo
              key={idx}
              title={action.title}
              subtitle={action.subtitle}
              avatarSrc={action.avatarSrc}
              className="!p-0 !bg-transparent"
            />
          ))}
        </div>

        {/* Code and ubication */}
        <div className="flex flex-col gap-3 mt-4">
          <IconText icon={<MapPinIcon className="w-8 h-8" />} text={building} className="!text-[26px]" />
          <IconText icon={<TicketIcon className="w-8 h-8" />} text={code} className="!text-[26px]" />
        </div>
      </div>
    </Panel>
  );
};
