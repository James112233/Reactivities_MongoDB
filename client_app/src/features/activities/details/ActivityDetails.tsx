import { Button, Card, Image } from "semantic-ui-react";
import { useAppContext } from "../../../app/context/AppContext";

export default function ActivityDetails() {
  const { selectedActivity, cancelSelectActivity, openForm } = useAppContext();
  return (
    <Card fluid>
      <Image src={`/assets/categoryImages/${selectedActivity!.category}.jpg`} />
      <Card.Content>
        <Card.Header>{selectedActivity!.title}</Card.Header>
        <Card.Meta>
          <span>{selectedActivity!.date}</span>
        </Card.Meta>
        <Card.Description>{selectedActivity!.description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths="2">
          <Button
            onClick={() => openForm(selectedActivity!.id)}
            basic
            color="blue"
            content="Edit"
          />
          <Button
            onClick={cancelSelectActivity}
            basic
            color="grey"
            content="Cancel"
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
}
