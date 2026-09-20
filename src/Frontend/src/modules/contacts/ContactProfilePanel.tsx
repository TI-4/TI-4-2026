import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { IconText } from '../../components/IconText';
import { Button } from '../../components/Button';
import BuildingIcon from '../../assets/svg/icons/icon_building.svg?react';
import PhoneIcon from '../../assets/svg/icons/icon_phone.svg?react';
import MailIcon from '../../assets/svg/icons/icon_mail.svg?react';

import type { ScheduleItem } from '../../interfaces/ScheduleItem';

export interface ContactProfilePanelProps {
  name: string;
  degree: string;
  role: string;
  department: string;
  office: string;
  email: string;
  phone: string;
  photoUrl?: string;
  studentAttention?: string[];
  schedules?: ScheduleItem[];
  onClose?: () => void;
  onViewOffice?: () => void;
}

export const ContactProfilePanel = ({
  name,
  degree,
  role,
  department,
  office,
  email,
  phone,
  photoUrl,
  studentAttention = [],
  schedules = [],
  onClose,
  onViewOffice,
}: ContactProfilePanelProps) => {
  return (
    <Panel color="white" withUctBorder={true} onClose={onClose} outerClassName="w-max" innerClassName="p-8 w-[800px] flex flex-col">
      {/* Top Section */}
      <div className="flex flex-row gap-8 items-start">
        {/* Photo */}
        <PhotoFrame src={photoUrl} className="w-64 h-64 rounded-2xl" />

        {/* Details */}
        <div className="flex flex-col flex-1">
          <h2 className="text-3xl font-medium text-gray-800 mb-2 uppercase">{name}</h2>

          <div className="flex flex-col gap-1 text-gray-700 text-lg mb-6">
            <p>{degree}</p>
            <p>{role}</p>
            <p>{department}</p>
          </div>

          <div className="flex flex-col gap-3 mb-6">
            <IconText icon={<BuildingIcon />} text={office} className="!text-lg" />
            <IconText icon={<MailIcon />} text={email} className="!text-lg" />
            <IconText icon={<PhoneIcon />} text={phone} className="!text-lg" />
          </div>

          <Button variant="solid" onClick={onViewOffice} className="w-max px-8">
            VER OFICINA
          </Button>
        </div>
      </div>

      {/* Separator */}
      <hr className="border-t-[6px] border-page-blue my-8 rounded-full" />

      {/* Bottom Section */}
      <div className="grid grid-cols-2 gap-8">
        {/* Students schedule */}
        <div className="flex flex-col gap-3">
          <h3 className="text-lg font-medium text-gray-800">Atención Estudiantes</h3>
          <div className="bg-page-gray-light p-4 rounded-xl flex flex-col gap-3 min-h-[150px]">
            {studentAttention.length > 0 ? (
              studentAttention.map((item, index) => (
                <div key={index} className="bg-white p-4 rounded-lg shadow-sm min-h-[4rem] text-gray-700 flex items-center">
                  {item}
                </div>
              ))
            ) : (
              <React.Fragment>
                <div className="bg-white p-4 rounded-lg shadow-sm min-h-[4rem]"></div>
                <div className="bg-white p-4 rounded-lg shadow-sm min-h-[4rem]"></div>
              </React.Fragment>
            )}
          </div>
        </div>

        {/* Schedule */}
        <div className="flex flex-col gap-3">
          <h3 className="text-lg font-medium text-gray-800">Horarios</h3>
          <div className="bg-page-gray-light p-4 rounded-xl flex flex-col gap-3 min-h-[150px]">
            {schedules.length > 0 ? (
              schedules.map((schedule) => (
                <div key={schedule.id} className="bg-white p-4 rounded-lg shadow-sm flex flex-col">
                  <span className="font-medium text-gray-800">{schedule.course}</span>
                  <span className="text-sm text-gray-600">{schedule.time}</span>
                  {schedule.assistant && (
                    <span className="text-sm text-gray-600">Ayudante: {schedule.assistant}</span>
                  )}
                </div>
              ))
            ) : (
              <div className="bg-white p-4 rounded-lg shadow-sm flex flex-col">
                <span className="font-medium text-gray-800">Cálculo I</span>
                <span className="text-sm text-gray-600">Lunes: 8:00 - 12:30 a.m</span>
                <span className="text-sm text-gray-600">Ayudante: Foyarzo</span>
              </div>
            )}
          </div>
        </div>
      </div>
    </Panel>
  );
};
