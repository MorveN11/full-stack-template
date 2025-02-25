import { API_PATH } from '@/api/constants';
import { notFound } from 'next/navigation';

interface Test {
  list: string;
}

export const dynamic = 'force-dynamic';

async function getTest() {
  const res = await fetch(`${API_PATH}/test`);

  const data: Test = await res.json();

  if (!data) notFound();

  return data;
}

export default async function Page() {
  const test = await getTest();

  return (
    <div>
      <div className="min-h-screen flex justify-center place-items-center text-h1">
        <p className="link">{test.list} Mundoo</p>
      </div>
    </div>
  );
}
