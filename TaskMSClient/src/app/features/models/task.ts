import { TaskFileModel } from "./taskFile";
import { TasksStatus } from "./taskStatusEnum";

export interface TaskModel{

    id: string;
    title: string;
    description:string;
    user:any;
    userName:string;
    status: TasksStatus;
    createAt: Date;
    fileUrls: TaskFileModel[];
    
    
}