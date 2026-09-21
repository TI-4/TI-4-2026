import { Routes, Route, Navigate } from 'react-router-dom';
import { AppLayout } from '../components/AppLayout';
import { MapView } from '../views/MapView';
import { IncidentsView } from '../views/IncidentsView';
import { ObjectsView } from '../views/ObjectsView';
import { ContactsView } from '../views/ContactsView';
import { ShowcaseView } from '../views/ShowcaseView';
import { LoginView } from '../views/LoginView';

export const AppRouter = () => {
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route path="/map" element={<MapView />} />
        <Route path="/reports" element={<IncidentsView />} />
        <Route path="/objects" element={<ObjectsView />} />
        <Route path="/contacts" element={<ContactsView />} />
        <Route path="/showcase" element={<ShowcaseView />} />
        <Route path="/login" element={<LoginView />} />
      </Route>

      <Route path="/" element={<Navigate to="/map" />} />
    </Routes>
  );
};
