import { Routes, Route, Navigate } from 'react-router-dom';
import { AppLayout } from '../components/AppLayout';
import { MapView } from '../views/MapView';
import { IncidentsView } from '../views/IncidentsView';
import { ObjectsView } from '../views/ObjectsView';
import { ContactsView } from '../views/ContactsView';
import { ShowcaseView } from '../views/ShowcaseView';
import { LoginView } from '../views/LoginView';
import { ProtectedRoute } from '../router/ProtectedRoute';
import { AdministrationView } from '../views/AdministrationView';

export const AppRouter = () => {
  return (
    <Routes>
      <Route path="/login" element={<LoginView />} />

      <Route element={<AppLayout />}>

        {/* Public general routes */}
        <Route path="/map" element={<MapView />} />
        <Route path="/reports" element={<IncidentsView />} />
        <Route path="/objects" element={<ObjectsView />} />
        <Route path="/reports" element={<IncidentsView />} />
        <Route path="/showcase" element={<ShowcaseView />} />

        {/* Protected general routes */}
        <Route element={<ProtectedRoute unauthorizedTo="/login" />}>
          <Route path="/contacts" element={<ContactsView />} />
        </Route>

        {/* Role-Restricted Routes */}
        <Route element={<ProtectedRoute allowedRoles={['ADMIN']} />}>
          <Route path="/administration" element={<AdministrationView />} />
        </Route>


      </Route>

      <Route path="*" element={<Navigate to="/map" />} />
    </Routes>
  );
};
