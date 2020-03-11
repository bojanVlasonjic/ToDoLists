import { ToDoItem} from './to-do-item.model';

export class ToDoList {

    id: string;
    title: string;
    position: number;
    endDate: Date;
    dtoItems: ToDoItem[];

    constructor() {
    }

}
