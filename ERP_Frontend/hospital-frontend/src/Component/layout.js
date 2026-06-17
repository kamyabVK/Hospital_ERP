import { Outlet } from "react-router-dom";
import Sidebar from "./sidebar";
import Navbar from "./navbar";


export default function Layout() {
  return (
    <div className="app-container">
      <Sidebar />
      <div className="main-content">
        <Navbar />
        <div className="page-body">
          <Outlet /> {/* yahan har master component render hoga */}
        </div>
      </div>
    </div>
  );
}