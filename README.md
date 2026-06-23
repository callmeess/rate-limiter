# Rate Limiting Algorithms

A collection of common rate limiting algorithms with explanations, implementations, and comparisons.

---

## Overview

Rate limiting is a technique used to control how many requests a user, client, or service can make within a specific period of time.

It helps systems:

- Prevent API abuse
- Protect backend services
- Avoid overload
- Improve reliability
- Ensure fair resource usage

---

# Algorithms

## 1. Fixed Window Counter

### Description

Counts requests inside fixed time windows.

Example:

```
Limit: 100 requests / minute

12:00:00 - 12:00:59
```

### How it works

1. Create a counter for each time window
2. Increment on every request
3. Reject requests after reaching the limit
4. Reset counter when the window expires

### Pros

- Simple
- Fast
- Low memory usage

### Cons

- Boundary burst problem

Example:

```
12:00:59 -> 100 requests
12:01:00 -> 100 requests

Total: 200 requests in 1 second
```

### Complexity

```
Time:  O(1)
Space: O(1)
```

---

# 2. Sliding Window Log

## Description

Stores timestamps of every request and removes expired requests.

Example:

```
Requests:

10:00:05
10:00:20
10:00:45
```

For every request:

1. Remove timestamps older than the window
2. Count remaining requests
3. Allow if under the limit

## Pros

- Very accurate
- No boundary problem

## Cons

- Uses more memory

## Complexity

```
Time:  O(log n)
Space: O(n)
```

---

# 3. Sliding Window Counter

## Description

A combination of fixed windows and sliding calculations.

Instead of storing every request, store counters from current and previous windows.

Example:

```
Previous window:
80 requests

Current window:
40 requests
```

The algorithm estimates the real number of requests based on time overlap.

## Pros

- Lower memory than sliding log
- More accurate than fixed window

## Cons

- Approximation

## Complexity

```
Time:  O(1)
Space: O(1)
```

---

# 4. Token Bucket

## Description

The token bucket algorithm gives users a bucket of tokens.

Each request consumes one token.

Tokens are added back at a fixed refill rate.

Example:

```
Bucket capacity: 10 tokens
Refill rate: 1 token/sec
```

Initial state:

```
[##########]
10 tokens
```

After requests:

```
[#####_____]
5 tokens
```

When empty:

```
[__________]

Request rejected
```

## Pros

- Supports bursts
- Very common in production systems
- Efficient

## Cons

- Slightly more complex

## Complexity

```
Time:  O(1)
Space: O(1)
```

---

# 5. Leaky Bucket

## Description

Requests enter a queue and leave at a constant rate.

Example:

```
Incoming Requests

       |
       v

+-------------+
|   Queue     |
+-------------+

       |
       v

Fixed processing rate
```

## Pros

- Smooth traffic flow
- Prevents spikes

## Cons

- Adds latency
- Requests may wait

## Complexity

```
Time:  O(1)
Space: O(n)
```

---

# 6. Concurrency Limiter

## Description

Limits the number of requests being processed at the same time.

Example:

```
Maximum active requests = 50
```

If 50 requests are running:

```
Request 51
    |
    v
Rejected or queued
```

## Pros

- Protects expensive operations
- Simple

## Cons

- Does not limit requests per second

## Complexity

```
Time:  O(1)
Space: O(1)
```

---

# Algorithm Comparison

| Algorithm | Accuracy | Burst Support | Memory |
|---|---|---|---|
| Fixed Window | Low | Yes | Low |
| Sliding Window Log | High | No | High |
| Sliding Window Counter | Medium | Yes | Low |
| Token Bucket | High | Yes | Low |
| Leaky Bucket | High | No | Medium |
| Concurrency Limit | N/A | N/A | Low |

---

# Distributed Rate Limiting

In real systems, multiple servers need a shared rate limit state.

Example:

```
              Client

                 |

           Load Balancer

        -------------------

        API     API     API

        -------------------

                 |

              Redis

        Shared Counters
```

Common storage:

- Redis
- Database
- Distributed cache

---

# Example Configuration

```json
{
  "algorithm": "token_bucket",
  "limit": 100,
  "window": "60s"
}
```

---

# Example Usage

```python
if rate_limiter.allow(user_id):
    handle_request()
else:
    return "429 Too Many Requests"
```

---

# Testing

Test cases:

- Normal traffic
- Burst traffic
- Multiple users
- Concurrent requests
- Distributed servers

Example:

```
Limit: 100 requests/minute

Input:
1000 requests

Expected:

Allowed: 100
Rejected: 900
```

---

# Benchmark Metrics

Measure:

- Requests per second
- Latency
- Memory usage
- Accuracy
- CPU usage

Example:

```
Algorithm        Performance

Token Bucket     Excellent
Fixed Window     Fast
Sliding Log      Accurate but expensive
```

---

# Future Improvements

- Redis implementation
- Distributed locking
- User-based quotas
- IP-based limits
- API gateway integration
- Adaptive rate limiting

---

# License

MIT