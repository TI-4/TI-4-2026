import { useUiState } from '../states/uiState';
import { ModalOverlay } from './ModalOverlay';
import { TimedActionCard } from '../modules/actions/TimedActionCard';

export const GlobalModals = () => {
  const activePanel = useUiState(state => state.activePanel);
  const closePanel = useUiState(state => state.closePanel);

  return (
    <>
      <ModalOverlay 
        isOpen={activePanel === 'saveMapChanges'} 
        onClose={closePanel}
      >
        <TimedActionCard
          title="Confirmar Cambios"
          description="Estos cambios serán públicos y visibles para todos los estudiantes. Por favor, revisa tus modificaciones antes de continuar."
          initialDurationMs={3000}
          options={[
            {
              label: "Cancelar",
              color: "gray",
              onClick: closePanel
            },
            {
              label: "Publicar Mapa",
              color: "green",
              onClick: () => {
                console.log("Mapa guardado exitosamente");
                closePanel();
              }
            }
          ]}
        />
      </ModalOverlay>

      <ModalOverlay 
        isOpen={activePanel === 'discardMapChanges'} 
        onClose={closePanel}
      >
        <TimedActionCard
          title="Descartar Cambios"
          description="Estás a punto de descartar todas las modificaciones realizadas en esta sesión. Esta acción no se puede deshacer."
          initialDurationMs={2000}
          options={[
            {
              label: "Cancelar",
              color: "gray",
              onClick: closePanel
            },
            {
              label: "Sí, descartar",
              color: "red",
              onClick: () => {
                console.log("Cambios descartados");
                closePanel();
              }
            }
          ]}
        />
      </ModalOverlay>
    </>
  );
};
