# 🧠 TicTacToe Q-Learning Sandbox (Package Abstraction Experiment)

A small experimental project exploring **reinforcement learning architecture design** through a simple Tic-Tac-Toe environment.

The main goal of this repository is not the game itself, but the **separation of concerns between:**
- the environment (Tic-Tac-Toe rules)
- the learning algorithm (Q-learning agent)
- and the abstraction layer (reusable package structure)

This project was built as a **sandbox for testing modular AI/ML design patterns in .NET-style package architecture.**

---

## 🧠 What This Project Demonstrates

### 🎮 TicTacToe Environment
A fully defined Tic-Tac-Toe game used as a deterministic reinforcement learning environment:

- State representation of the board
- Valid action masking
- Win / loss / draw evaluation
- Turn-based simulation

---

### 🤖 Q-Learning Agent (Pluggable Design)

The learning agent is designed as an independent module:

- Q-table based learning
- Reward-driven updates
- Exploration vs exploitation (ε-greedy strategy)
- Stateless interaction with the environment

---

### 📦 Package Abstraction Layer

A key focus of this project is architecture:

- Environment does NOT know about learning
- Agent does NOT know game rules internally
- Training loop orchestrates interaction
- Modules can be swapped or reused in other environments

This makes the system reusable.

## 🧱 Architecture Overview
