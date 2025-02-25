import { NextResponse } from 'next/server';

export async function GET() {
  try {
    return NextResponse.json({ status: 'ok', message: 'Service is healthy' }, { status: 200 });
  } catch (_) {
    return NextResponse.json({ status: 'error', message: 'Health check failed' }, { status: 500 });
  }
}
