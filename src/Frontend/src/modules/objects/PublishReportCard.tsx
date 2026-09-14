import { useState } from 'react';
import { Panel } from '../../components/Panel';
import { Input } from '../../components/Input';
import { Select } from '../../components/Select';
import { FileUpload } from '../../components/FileUpload';
import { Button } from '../../components/Button';
import { CloseButton } from '../../components/ExitButton';

const BuildingIcon = () => (
  <svg className="w-5 h-5 text-gray-700" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 2L2 7v15h20V7L12 2zm0 2.5l7 3.5v12h-4v-5H9v5H5v-12l7-3.5zm-5 5v2h2v-2H7zm4 0v2h2v-2h-2zm4 0v2h2v-2h-2z" />
  </svg>
);

interface PublishReportCardProps {
  onPublish?: (data: { title: string; description: string; campus: string; building: string; files: File[] }) => void;
  onDiscard?: () => void;
}

export const PublishReportCard = ({ onPublish, onDiscard }: PublishReportCardProps) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [campus, setCampus] = useState('san-juan-pablo-ii');
  const [building, setBuilding] = useState('edificio-11');
  const [files, setFiles] = useState<File[]>([]);

  const campusOptions = [
    { value: 'san-juan-pablo-ii', label: 'Campus San Juan Pablo II' },
    { value: 'san-francisco', label: 'Campus San Francisco' },
    { value: 'menchaca-lira', label: 'Campus Menchaca Lira' },
  ];

  const buildingOptions = [
    { value: 'edificio-11', label: 'Edificio 11' },
    { value: 'edificio-a', label: 'Edificio A' },
    { value: 'edificio-b', label: 'Edificio B' },
    { value: 'biblioteca', label: 'Biblioteca Central' },
  ];

  const handlePublish = () => {
    if (onPublish) {
      onPublish({ title, description, campus, building, files });
    }
  };

  return (
    <Panel withUctBorder color="white" outerClassName="rounded-3xl max-w-2xl w-full" innerClassName="p-8 flex flex-col gap-5">
      <div className="flex items-center justify-between w-full">
        <h2 className="text-3xl font-black text-gray-800 tracking-tight">PUBLICAR REPORTE</h2>
        <CloseButton variant="ghost" size="lg" onClick={onDiscard} />
      </div>

      <Input
        labelColor="text-gray-700"
        placeholder="TITULO"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      <Input
        labelColor="text-gray-700"
        multiline
        placeholder="DESCRIPCIÓN"
        rows={3}
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />

      <FileUpload
        maxFiles={3}
        label="ARRASTRA MÁXIMO 3 IMÁGENES"
        height="h-52"
        onFilesChange={setFiles}
      />

      <div className="flex flex-wrap sm:flex-nowrap gap-4">
        <div className="flex-1 min-w-[200px]">
          <Select
            labelColor="text-gray-700"
            options={campusOptions}
            value={campus}
            onChange={setCampus}
            icon={<BuildingIcon />}
          />
        </div>

        <div className="flex-1 min-w-[180px]">
          <Select
            labelColor="text-gray-700"
            options={buildingOptions}
            value={building}
            onChange={setBuilding}
            icon={<BuildingIcon />}
          />
        </div>
      </div>

      <p className="text-xs text-gray-600 leading-relaxed font-medium">
        La publicación de reportes falsos, malintencionados o con contenido inapropiado puede acarrear sanciones, bloqueo de cuenta o la suspensión de privilegios en el sistema.
      </p>

      <div className="flex items-center justify-end gap-4 pt-2">
        <Button
          color="red"
          variant="solid"
          size="md"
          width="md"
          onClick={onDiscard}
          className="font-bold tracking-wide"
        >
          Descartar
        </Button>

        <Button
          color="blue"
          variant="solid"
          size="md"
          width="md"
          onClick={handlePublish}
          className="font-bold tracking-wide"
        >
          Publicar
        </Button>
      </div>
    </Panel>
  );
};
