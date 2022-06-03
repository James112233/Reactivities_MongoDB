import {
  createContext,
  PropsWithChildren,
  useContext,
  useEffect,
  useState,
} from "react";
import agent from "../api/agent";
import { Activity } from "../models/activity";
import { v4 as uuid } from "uuid";

interface AppContextValue {
  activities: Activity[] | null;
  selectedActivity: Activity | undefined;
  selectActivity: (id: string) => void;
  cancelSelectActivity: () => void;
  editMode: boolean;
  openForm: (id?: string) => void;
  closeForm: () => void;
  createOrEditActivity: (activity: Activity) => void;
  deleteActivity: (id: string) => void;
  submitting: boolean;
  loading: boolean;
}

export const AppContext = createContext<AppContextValue | undefined>(undefined);

export function useAppContext() {
  const context = useContext(AppContext);

  if (context === undefined) {
    throw Error("Oops - we do not seem to be inside the provider");
  }

  return context;
}

export function AppProvider({ children }: PropsWithChildren<any>) {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<
    Activity | undefined
  >(undefined);
  const [editMode, setEditMode] = useState(false);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  //useEffect is called on every re-render
  useEffect(() => {
    agent.Activities.list().then((response) => {
      // console.log(response);
      let activities: Activity[] = [];
      response.activities.forEach((activity: Activity) => {
        activity.date = activity.date.split("T")[0];
        activities.push(activity);
      });
      setActivities(activities);
      setLoading(false);
    });
  }, []); //<--- array of dependencies, to prevent the infinite call back

  function selectActivity(id: string) {
    setSelectedActivity(activities.find((x) => x.id === id));
  }

  function cancelSelectActivity() {
    setSelectedActivity(undefined);
  }

  function openForm(id?: string) {
    id ? selectActivity(id) : cancelSelectActivity();
    setEditMode(true);
  }

  function closeForm() {
    setEditMode(false);
  }

  function createOrEditActivity(activity: Activity) {
    setSubmitting(true);
    if (activity.id) {
      agent.Activities.update(activity).then(() => {
        setActivities([
          ...activities.filter((x) => x.id !== activity.id),
          activity,
        ]);
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmitting(false);
      });
    } else {
      activity.id = uuid();
      agent.Activities.create(activity).then(() => {
        setActivities([...activities, activity]);
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmitting(false);
      });
    }
  }

  function deleteActivity(id: string) {
    setSubmitting(true);
    agent.Activities.delete(id).then(() => {
      setActivities([...activities.filter((x) => x.id !== id)]);
      setSubmitting(false);
    });
  }

  return (
    <AppContext.Provider
      value={{
        activities,
        selectedActivity,
        editMode,
        openForm,
        closeForm,
        selectActivity,
        cancelSelectActivity,
        createOrEditActivity,
        deleteActivity,
        loading,
        submitting,
      }}
    >
      {children}
    </AppContext.Provider>
  );
}
