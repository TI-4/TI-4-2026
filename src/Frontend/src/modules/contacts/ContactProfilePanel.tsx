import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { IconText } from '../../components/IconText';
import { Button } from '../../components/Button';

const BuildingIcon = () => (
  <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

const MailIcon = () => (
  <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
    <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
  </svg>
);

const PhoneIcon = () => (
  <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
    <path d="M6.62 10.79c1.44 2.83 3.76 5.14 6.59 6.59l2.2-2.2c.27-.27.67-.36 1.02-.24 1.12.37 2.33.57 3.57.57.55 0 1 .45 1 1V20c0 .55-.45 1-1 1-9.39 0-17-7.61-17-17 0-.55.45-1 1-1h3.5c.55 0 1 .45 1 1 0 1.25.2 2.45.57 3.57.11.35.03.74-.25 1.02l-2.2 2.2z" />
  </svg>
);

export interface ScheduleItem {
  id: string;
  course: string;
  time: string;
  assistant?: string;
}

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
