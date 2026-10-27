const czechDateFormat = new Intl.DateTimeFormat("cs-CZ", {
  day: "numeric",
  month: "numeric",
  year: "numeric"
});

const czechDateTimeFormat = new Intl.DateTimeFormat("cs-CZ", {
  second: "numeric",
  minute: "numeric",
  hour: "numeric",
  day: "numeric",
  month: "numeric",
  year: "numeric"
});

export function toCzechDateTimeString(value?: Date | string, time = false): string {
  if (!value) {
    return "";
  }

  if (typeof value === "string") {
    value = new Date(value);
  }

  if (time) {
    return czechDateTimeFormat.format(value);
  }

  return czechDateFormat.format(value);
}