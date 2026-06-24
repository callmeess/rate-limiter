import http from "k6/http";

export const options = {
  scenarios: {
    steady_load: {
      executor: "constant-arrival-rate",
      rate: 100,      // requests/sec
      timeUnit: "1s",
      duration: "5s",
      preAllocatedVUs: 200,
      maxVUs: 1000,
    },
  },
};


export default function () {
  const res = http.get(
    "http://host.docker.internal:5038/Temperature/weatherforecast"
  );

  console.log(res.status);
}