import { Container } from "semantic-ui-react";
import NavBar from "./Navbar";
import ActivityDashBoard from "../../features/activities/dashborad/ActivityDashBoard";
import { useAppContext } from "../context/AppContext";
import LoadingComponent from "./LoadingComponent";

function App() {
  const { loading } = useAppContext();

  if (loading) return <LoadingComponent content="Loading activities..." />;

  return (
    <>
      <NavBar />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashBoard />
      </Container>
    </>
  );
}

export default App;
