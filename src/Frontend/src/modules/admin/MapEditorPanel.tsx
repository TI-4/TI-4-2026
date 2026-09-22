import { useState, useEffect } from 'react';
import { SquareButton } from '../../components/SquareButton';
import { Button } from '../../components/Button';

import PencilIcon from '../../assets/svg/icons/icon_pencil.svg?react';
import MapPinIcon from '../../assets/svg/icons/icon_location.svg?react';
import TrashIcon from '../../assets/svg/icons/icon_trash.svg?react';
import CheckIcon from '../../assets/svg/icons/icon_check.svg?react';
import CrossIcon from '../../assets/svg/icons/icon_cross.svg?react';

type EditorMode = 'idle' | 'adding' | 'deleting';

export const MapEditorPanel = () => {
  const [mode, setMode] = useState<EditorMode>('idle');
  const [displayMode, setDisplayMode] = useState<EditorMode>('adding'); // Saves the last state for animation

  // Updates visible text only when entering a mode (not when exiting)
  useEffect(() => {
    if (mode !== 'idle') {
      setDisplayMode(mode);
    }
  }, [mode]);

  return (
    <div className="bg-white p-3 rounded-2xl shadow-sm border border-gray-200 inline-block min-w-[360px]">

      {/* Panel Title */}
      <div className="flex items-center gap-2 mb-2 px-1">
        <PencilIcon className="w-4 h-4 text-gray-700" />
        <h2 className="font-semibold text-gray-700 text-sm">Editor de Mapa</h2>
      </div>

      {/* Animated Buttons/Modes Container */}
      <div className="relative overflow-hidden transition-all duration-300 ease-in-out" style={{ minHeight: '64px' }}>

        {/* IDLE MODE (The 4 buttons) */}
        <div
          className={`absolute inset-0 flex items-center justify-center gap-3 transition-all duration-300 ease-in-out ${
            mode === 'idle' ? 'opacity-100 translate-x-0 pointer-events-auto' : 'opacity-0 -translate-x-10 pointer-events-none'
          }`}
        >
          {/* Add Point */}
          <SquareButton
            title="Añadir un punto"
            onClick={() => setMode('adding')}
          >
            <MapPinIcon className="w-8 h-8" />
          </SquareButton>

          {/* Delete Point */}
          <SquareButton
            title="Eliminar un punto"
            onClick={() => setMode('deleting')}
          >
            <TrashIcon className="w-8 h-8" />
          </SquareButton>

          <div className="w-px h-10 bg-gray-200 mx-1"></div> {/* Visual Separator */}

          {/* Save Changes (Green) */}
          <SquareButton
            title="Guardar cambios"
            className="!text-green-600 hover:!bg-green-50 active:!bg-green-600 active:!text-white"
          >
            <CheckIcon className="w-8 h-8" />
          </SquareButton>

          {/* Discard Changes (Red) */}
          <SquareButton
            title="Descartar cambios"
            className="!text-red-600 hover:!bg-red-50 active:!bg-red-600 active:!text-white"
          >
            <CrossIcon className="w-8 h-8" />
          </SquareButton>
        </div>

        {/* EDIT MODE (Adding / Deleting) */}
        <div
          className={`absolute inset-0 flex items-center justify-between gap-6 px-2 bg-gray-50 rounded-2xl border border-gray-200 transition-all duration-300 ease-in-out ${
            mode !== 'idle' ? 'opacity-100 translate-x-0 pointer-events-auto' : 'opacity-0 translate-x-10 pointer-events-none'
          }`}
        >
          <div className="flex items-center gap-3">
            <div className={`p-2 rounded-full ${displayMode === 'adding' ? 'bg-blue-100 text-blue-600' : 'bg-red-100 text-red-600'}`}>
              {displayMode === 'adding' ? <MapPinIcon className="w-5 h-5" /> : <TrashIcon className="w-5 h-5" />}
            </div>
            <p className="font-medium text-gray-700 text-sm">
              {displayMode === 'adding'
                ? 'Haz clic en el mapa para colocar el punto.'
                : 'Haz clic en un punto del mapa para eliminarlo.'}
            </p>
          </div>

          <Button
            variant="solid"
            color={displayMode === 'adding' ? 'blue' : 'red'}
            size="sm"
            className="!w-auto !px-4 shadow-none"
            onClick={() => setMode('idle')}
          >
            Terminar
          </Button>
        </div>

      </div>

    </div>
  );
};
