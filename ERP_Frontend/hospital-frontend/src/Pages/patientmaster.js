import MasterTable from "../Component/mastertable";

const columns = [
  { key: "name", label: "Patient Name" },
  { key: "age", label: "Age" },
  { key: "ward", label: "Ward" },
];

const initialData = [
  { id: 1, name: "Rohan Sharma", age: 34, ward: "General" },
];

export default function PatientMaster() {
  return <MasterTable title="Patient Master" columns={columns} initialData={initialData} />;
}