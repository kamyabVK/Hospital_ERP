import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from './Component/layout';
import Dashboard from './Pages/dashboard';
import PatientMaster from './Pages/patientmaster';
import DoctorMaster from './Pages/doctormaster';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<Dashboard />} />
          <Route path="/patient-master" element={<PatientMaster />} />
          <Route path="/doctor-master" element={<DoctorMaster />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}