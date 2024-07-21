import { TasksStatus } from "./taskStatusEnum";

export interface TaskFilterModel {
    status?: TasksStatus;
    startDate?: Date;
    endDate?: Date;
    userName?: string; 
    sortOrder?: 'asc' | 'desc'; 
  }
  