create DATABASE freelancer_bd;

create type order_status as enum ('open', 'processing', 'close');

alter type order_status owner to postgres;

create type application_status as enum ('open', 'accepted', 'rejected');

alter type application_status owner to postgres;

create type work_status as enum ('open', 'processing', 'done', 'close');

alter type work_status owner to postgres;

create table users
(
    id       serial      not null,
    login    varchar(32) not null,
    password varchar(60) not null,
    token    varchar,
    constraint users_pk
        primary key (id)
);

alter table users
    owner to postgres;

create unique index users_login_uindex
    on users (login);

create table info
(
    id            serial not null,
    first_name    varchar(64),
    second_name   varchar(64),
    date_of_birth date,
    additional    varchar,
    constraint info_pk
        primary key (id)
);

alter table info
    owner to postgres;

create table freelancers
(
    id      serial  not null,
    user_id integer not null,
    info_id integer not null,
    constraint freelancers_pk
        primary key (id),
    constraint user_fk
        foreign key (user_id) references users,
    constraint info_fk
        foreign key (info_id) references info
);

alter table freelancers
    owner to postgres;

create table customers
(
    id      serial  not null,
    user_id integer not null,
    info_id integer not null,
    constraint " customer_pk"
        primary key (id),
    constraint info_fk
        foreign key (info_id) references info,
    constraint user_fk
        foreign key (user_id) references users
);

alter table customers
    owner to postgres;

create table orders
(
    id          serial       not null,
    customer_id integer      not null,
    order_date  date,
    name        varchar      not null,
    "desc"      varchar,
    status      order_status not null,
    constraint orders_pk
        primary key (id),
    constraint customer_fk
        foreign key (customer_id) references customers
);

alter table orders
    owner to postgres;

create table applications
(
    id            serial  not null,
    freelancer_id integer not null,
    apply_date    date,
    order_id      integer not null,
    status        application_status,
    constraint applications_pk
        primary key (id),
    constraint freelancer_fk
        foreign key (freelancer_id) references freelancers,
    constraint order_fk
        foreign key (order_id) references orders
);

alter table applications
    owner to postgres;

create table works
(
    id            serial      not null,
    order_id      integer     not null,
    freelancer_id integer     not null,
    status        work_status not null,
    work_date     date,
    constraint works_pk
        primary key (id),
    constraint order_fk
        foreign key (order_id) references orders,
    constraint freelancer_fk
        foreign key (freelancer_id) references freelancers
);

alter table works
    owner to postgres;

INSERT INTO public.info (id, first_name, second_name, date_of_birth, additional) VALUES (1, 'Denis', 'Korolev', '1999-10-15', null);
INSERT INTO public.info (id, first_name, second_name, date_of_birth, additional) VALUES (2, 'Valerii', 'Kvan', '1997-01-29', null);
INSERT INTO public.info (id, first_name, second_name, date_of_birth, additional) VALUES (3, 'Jozeph', 'Stulin', '1987-12-03', null);

INSERT INTO public.users (id, login, password, token) VALUES (3, 'user', 'ee11cbb19052e40b07aac0ca060c23ee', null);
INSERT INTO public.users (id, login, password, token) VALUES (1, 'glitchyhydra', '9cea10c7ff109c6e61727a0d45492ead', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjEiLCJSb2xlIjoiRnJlZWxhbmNlciIsIm5iZiI6MTYxMTA3NzU1NiwiZXhwIjoxNjExNjgyMzU2LCJpYXQiOjE2MTEwNzc1NTYsImlzcyI6IkZyZWVsYW5jZXJBdXRoU2VydmVyIiwiYXVkIjoiRnJlZWxhbmNlcnMifQ.K6puc2ci0eqZt0dR-rGKuV_CNNBCQc-yni81IdbTzyY');
INSERT INTO public.users (id, login, password, token) VALUES (2, 'admin', '21232f297a57a5a743894a0e4a801fc3', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjIiLCJSb2xlIjoiQ3VzdG9tZXIiLCJuYmYiOjE2MTg3NzQxMDAsImV4cCI6MTYxOTM3ODkwMCwiaWF0IjoxNjE4Nzc0MTAwLCJpc3MiOiJGcmVlbGFuY2VyQXV0aFNlcnZlciIsImF1ZCI6IkZyZWVsYW5jZXJzIn0.1S5zQ9a-kLhAp4ohLQf1bY5sBRHUROL7KeQkXhkp89U');

INSERT INTO public.customers (id, user_id, info_id) VALUES (1, 2, 3);
INSERT INTO public.freelancers (id, user_id, info_id) VALUES (1, 1, 2);

INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (2, 1, '2021-01-16', 'vdavasva', '', 'open');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (10, 1, '2021-01-19', 'test finf', 'description for loosers', 'close');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (9, 1, '2021-01-19', 'test neffa', '', 'close');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (8, 1, '2021-01-19', 'mega nega ', 'no desc', 'processing');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (7, 1, '2021-01-19', 'test1', '', 'close');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (1, 1, '2021-01-05', 'write web page', '', 'close');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (6, 1, '2021-01-19', 'test1', '', 'close');
INSERT INTO public.orders (id, customer_id, order_date, name, "desc", status) VALUES (11, 1, '2021-01-19', 'test123 test321', 'neasdasd', 'open');

INSERT INTO public.applications (id, freelancer_id, apply_date, order_id, status) VALUES (1, 1, '2021-01-19', 1, 'open');
INSERT INTO public.applications (id, freelancer_id, apply_date, order_id, status) VALUES (2, 1, '2021-01-19', 1, 'accepted');
INSERT INTO public.applications (id, freelancer_id, apply_date, order_id, status) VALUES (3, 1, '2021-01-19', 6, 'accepted');

INSERT INTO public.works (id, order_id, freelancer_id, status, work_date) VALUES (1, 1, 1, 'done', '2021-01-19');
INSERT INTO public.works (id, order_id, freelancer_id, status, work_date) VALUES (2, 6, 1, 'done', '2021-01-19');
