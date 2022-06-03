import { Grid } from "semantic-ui-react";
import { useAppContext } from "../../../app/context/AppContext";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";
import ActivityList from "./ActivitiyList";

export default function ActivityDashBoard() {
  const {
    activities,
    selectedActivity,
    editMode,
  } = useAppContext();

  return (
    <Grid>
      <Grid.Column width="10">
        {activities!.length !== 0 ? (
          <ActivityList />
        ) : (
          <span>No Data Found.</span>
        )}
      </Grid.Column>
      <Grid.Column width="6">
        {selectedActivity && !editMode && <ActivityDetails />}
        {editMode && <ActivityForm />}
      </Grid.Column>
    </Grid>
  );
}
