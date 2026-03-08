#!/usr/bin/env bash

set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

default_message="Update project"
commit_message="${1:-$default_message}"
branch_name="$(git branch --show-current)"

if ! command -v gh >/dev/null 2>&1; then
  echo "gh is not installed."
  exit 1
fi

if ! gh auth status >/dev/null 2>&1; then
  echo "GitHub CLI is not logged in."
  echo "Run: gh auth login"
  exit 1
fi

if ! git remote get-url origin >/dev/null 2>&1; then
  echo "Git remote origin is not configured."
  exit 1
fi

git add .

if git diff --cached --quiet; then
  echo "No staged changes to commit."
else
  git commit -m "$commit_message"
fi

git push -u origin "$branch_name"

echo
echo "Pushed to $(git remote get-url origin) on branch $branch_name"
