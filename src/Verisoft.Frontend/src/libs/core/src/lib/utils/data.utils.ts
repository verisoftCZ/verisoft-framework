import moment from 'moment';

export function transformData(data: any){
  if (data) {
    if (moment.isMoment(data)) {
      return moment(data)
        .toDate()
        .toISOString();
    }
    else if (data instanceof Date) {
      return data.toISOString();
    }
  }
  return data;
}

export function convertToDate(data: any){
  if (data) {
    if (!isNaN(Date.parse(data))) {
      return new Date(data);
    }
  }
  return data;
}

export function convertToDateTime(data: any){
  if (data) {
    if (!isNaN(Date.parse(data))) {
      const date = new Date(data);
      return moment(date).format('DD.MM.yyyy HH:mm');
    }
  }
  return data;
}