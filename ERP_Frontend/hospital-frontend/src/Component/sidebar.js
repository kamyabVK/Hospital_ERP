import { NavLink } from "react-router-dom";
import "./sidebar.css";

const menuItems = [
  { name: "Dashboard", path: "/" },
  { name: "Patient Master", path: "/patient-master" },
  { name: "Doctor Master", path: "/doctor-master" },
  { name: "Medicine Master", path: "/medicine-master" },
  { name: "Ward Master", path: "/ward-master" },
];

export default function Sidebar() {
  return (
    <div className="sidebar">
      <div className="sidebar-logo">Hospital ERP</div>
      <ul className="sidebar-menu">
        {menuItems.map((item) => (
          <li key={item.path}>
            <NavLink
              to={item.path}
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              {item.name}
            </NavLink>
          </li>
        ))}
      </ul>
    </div>
  );
}