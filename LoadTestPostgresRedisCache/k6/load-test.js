import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '5s', target: 10 }, // ramp up to 10 users
        { duration: '10s', target: 10 }, // stay at 10 users
        { duration: '5s', target: 0 }, // ramp down
    ],
};

export default function () {
    const payload = JSON.stringify({
        title: `Todo ${Math.random()}`,
        id: Math.random() * 100 | 0
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.post('https://localhost:63622/todo', payload, params);

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
