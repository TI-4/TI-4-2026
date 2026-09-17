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

const TicketIcon = () => (
  <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5">
    <path strokeLinecap="round" strokeLinejoin="round" d="M16.5 6v.75m0 3v.75m0 3v.75m0 3V18m-9-5.25h5.25M7.5 15h3M3.375 5.25c-.621 0-1.125.504-1.125 1.125v3.026a2.999 2.999 0 010 5.198v3.026c0 .621.504 1.125 1.125 1.125h17.25c.621 0 1.125-.504 1.125-1.125v-3.026a2.999 2.999 0 010-5.198V6.375c0-.621-.504-1.125-1.125-1.125H3.375z" />
  </svg>
);

export interface ObjectReportAction {
  title: string;
  subtitle: string;
  avatarSrc?: string;
}

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
          <IconText icon={<MapPinIcon />} text={building} className="!text-[26px]" />
          <IconText icon={<TicketIcon />} text={code} className="!text-[26px]" />
        </div>
      </div>
    </Panel>
  );
};
