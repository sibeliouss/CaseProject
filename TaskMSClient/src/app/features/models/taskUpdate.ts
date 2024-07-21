import { TasksStatus } from "./taskStatusEnum";

export interface TaskUpdateModel {
    Id: string;
    Title: string;
    Description: string;
    Status: TasksStatus;
}