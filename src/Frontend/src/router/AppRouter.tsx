import { Routes, Route, Navigate } from 'react-router-dom';
import { AppLayout } from '../components/AppLayout';
import { MapView } from '../views/MapView';
import { IncidentsView } from '../views/IncidentsView';
import { ObjectsView } from '../views/ObjectsView';
import { ContactsView } from '../views/ContactsView';
import { ShowcaseView } from '../views/ShowcaseView';
import { LoginView } from '../views/LoginView';
import { ProtectedRoute } from '../router/ProtectedRoute';

export const AppRouter = () => {
  return (
    <Routes>
      <Route path="/login" element={<LoginView />} />

      <Route element={<AppLayout />}>

        {/* Public general routes */}
        <Route path="/map" element={<MapView />} />
        <Route path="/reports" element={<IncidentsView />} />
        <Route path="/objects" element={<ObjectsView />} />
        <Route path="/showcase" element={<ShowcaseView />} />

        {/* Protected general routes */}
        <Route element={<ProtectedRoute requireAuth={true} />}>
          <Route path="/contacts" element={<ContactsView />} />
        </Route>

        {/* Role-Restricted Routes */}


      </Route>

      <Route path="*" element={<Navigate to="/map" />} />
    </Routes>
  );
};
